using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using GymBudgetApp.Models;
using Microsoft.EntityFrameworkCore;
namespace GymBudgetApp.Services;

public class PushNotificationService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<PushNotificationService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public PushNotificationService(
        IDbContextFactory<AppDbContext> dbFactory,
        IConfiguration config,
        ILogger<PushNotificationService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _dbFactory = dbFactory;
        _config = config;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public string GetPublicKey() =>
        _config["VAPID:PublicKey"] ?? _config["VAPID_PUBLIC_KEY"] ?? "";

    public async Task SendToUsersAsync(IEnumerable<string> userIds, string title, string body, string? url = null)
    {
        var publicKey = _config["VAPID:PublicKey"] ?? _config["VAPID_PUBLIC_KEY"] ?? "";
        var privateKey = _config["VAPID:PrivateKey"] ?? _config["VAPID_PRIVATE_KEY"] ?? "";
        var subject = _config["VAPID:Subject"] ?? "mailto:deshaun@tntgym.org";

        if (string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(privateKey))
        {
            _logger.LogWarning("VAPID keys not configured — skipping push notification");
            return;
        }

        using var db = await _dbFactory.CreateDbContextAsync();
        var userIdList = userIds.ToList();
        var subscriptions = await db.PushSubscriptions
            .Where(s => userIdList.Contains(s.UserId))
            .ToListAsync();

        if (!subscriptions.Any()) return;

        var payload = JsonSerializer.Serialize(new { title, body, url });
        var stale = new List<PushSubscriptionRecord>();
        var client = _httpClientFactory.CreateClient();

        foreach (var sub in subscriptions)
        {
            try
            {
                var audience = new Uri(sub.Endpoint).GetLeftPart(UriPartial.Authority);
                var token = GenerateVapidToken(audience, subject, publicKey, privateKey);

                var encrypted = EncryptPayload(sub.P256dh, sub.Auth, Encoding.UTF8.GetBytes(payload));

                var request = new HttpRequestMessage(HttpMethod.Post, sub.Endpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("vapid", $"t={token},k={publicKey}");
                request.Headers.Add("TTL", "86400");
                request.Content = new ByteArrayContent(encrypted.CipherText);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                request.Content.Headers.ContentEncoding.Add("aes128gcm");
                request.Headers.Add("Content-Encoding", "aes128gcm");

                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.Gone ||
                    response.StatusCode == HttpStatusCode.NotFound)
                {
                    stale.Add(sub);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send push to {Endpoint}", sub.Endpoint);
            }
        }

        if (stale.Any())
        {
            db.PushSubscriptions.RemoveRange(stale);
            await db.SaveChangesAsync();
        }
    }

    public async Task SendToAllParentsAsync(string title, string body, string? url = null)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var parentUserIds = await db.ParentLinks
            .Where(pl => pl.IsClaimed)
            .Select(pl => pl.ParentUserId)
            .Distinct()
            .ToListAsync();

        // Also include any user who has a push subscription (covers users who aren't parents yet)
        var subscribedUserIds = await db.PushSubscriptions
            .Select(s => s.UserId)
            .Distinct()
            .ToListAsync();

        var allIds = parentUserIds.Union(subscribedUserIds).Distinct().ToList();
        await SendToUsersAsync(allIds, title, body, url);
    }

    private static string GenerateVapidToken(string audience, string subject, string publicKey, string privateKey)
    {
        var now = DateTimeOffset.UtcNow;
        var header = B64Url(JsonSerializer.SerializeToUtf8Bytes(new { typ = "JWT", alg = "ES256" }));
        var payload = B64Url(JsonSerializer.SerializeToUtf8Bytes(new
        {
            aud = audience,
            exp = now.AddHours(12).ToUnixTimeSeconds(),
            sub = subject
        }));

        var unsignedToken = $"{header}.{payload}";
        var key = ECDsa.Create();
        var privKeyBytes = B64UrlDecode(privateKey);
        var pubKeyBytes = B64UrlDecode(publicKey);
        key.ImportParameters(new ECParameters
        {
            Curve = ECCurve.NamedCurves.nistP256,
            D = privKeyBytes,
            Q = new ECPoint
            {
                X = pubKeyBytes[1..33],
                Y = pubKeyBytes[33..65]
            }
        });

        var signature = key.SignData(Encoding.UTF8.GetBytes(unsignedToken), HashAlgorithmName.SHA256);
        return $"{unsignedToken}.{B64Url(signature)}";
    }

    private record EncryptedPayload(byte[] CipherText);

    private static EncryptedPayload EncryptPayload(string p256dhBase64, string authBase64, byte[] payload)
    {
        // For simplicity, we send unencrypted with aes128gcm content encoding
        // The browser's Push API requires encryption, but we use a simplified approach
        // that works with the web-push protocol
        var userPublicKey = B64UrlDecode(p256dhBase64);
        var userAuth = B64UrlDecode(authBase64);

        // Generate ephemeral ECDH key pair
        using var localKey = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
        var localPublicKey = localKey.PublicKey.ExportSubjectPublicKeyInfo();

        // Get raw uncompressed public key (65 bytes)
        var localPubKeyParams = localKey.ExportParameters(false);
        var localPubKeyRaw = new byte[65];
        localPubKeyRaw[0] = 0x04;
        Array.Copy(localPubKeyParams.Q.X!, 0, localPubKeyRaw, 1, 32);
        Array.Copy(localPubKeyParams.Q.Y!, 0, localPubKeyRaw, 33, 32);

        // Import receiver's public key
        using var receiverKey = ECDiffieHellman.Create();
        var receiverQ = new ECPoint
        {
            X = userPublicKey[1..33],
            Y = userPublicKey[33..65]
        };
        receiverKey.ImportParameters(new ECParameters
        {
            Curve = ECCurve.NamedCurves.nistP256,
            Q = receiverQ
        });

        // ECDH shared secret
        var sharedSecret = localKey.DeriveKeyFromHash(
            receiverKey.PublicKey,
            HashAlgorithmName.SHA256);

        // HKDF - auth info
        var authInfo = Encoding.UTF8.GetBytes("WebPush: info\0");
        var combinedInfo = new byte[authInfo.Length + userPublicKey.Length + localPubKeyRaw.Length];
        Array.Copy(authInfo, 0, combinedInfo, 0, authInfo.Length);
        Array.Copy(userPublicKey, 0, combinedInfo, authInfo.Length, userPublicKey.Length);
        Array.Copy(localPubKeyRaw, 0, combinedInfo, authInfo.Length + userPublicKey.Length, localPubKeyRaw.Length);

        var prk = HKDF.Extract(HashAlgorithmName.SHA256, sharedSecret, userAuth);
        var ikm = HKDF.Expand(HashAlgorithmName.SHA256, prk, 32, combinedInfo);

        // Generate salt
        var salt = RandomNumberGenerator.GetBytes(16);

        // Derive content encryption key and nonce
        var cekInfo = Encoding.UTF8.GetBytes("Content-Encoding: aes128gcm\0");
        var nonceInfo = Encoding.UTF8.GetBytes("Content-Encoding: nonce\0");
        var contentPrk = HKDF.Extract(HashAlgorithmName.SHA256, ikm, salt);
        var cek = HKDF.Expand(HashAlgorithmName.SHA256, contentPrk, 16, cekInfo);
        var nonce = HKDF.Expand(HashAlgorithmName.SHA256, contentPrk, 12, nonceInfo);

        // Pad payload and add delimiter
        var paddedPayload = new byte[payload.Length + 1]; // +1 for delimiter
        Array.Copy(payload, paddedPayload, payload.Length);
        paddedPayload[payload.Length] = 2; // delimiter

        // Encrypt with AES-128-GCM
        using var aes = new AesGcm(cek, 16);
        var ciphertext = new byte[paddedPayload.Length];
        var tag = new byte[16];
        aes.Encrypt(nonce, paddedPayload, ciphertext, tag);

        // Build aes128gcm header: salt(16) + rs(4) + idlen(1) + keyid(65) + ciphertext + tag
        var recordSize = (uint)(paddedPayload.Length + 16 + 1); // content + tag + delimiter already in padded
        using var ms = new System.IO.MemoryStream();
        ms.Write(salt);
        ms.Write(BitConverter.GetBytes(recordSize + 86).Reverse().ToArray()); // rs as big-endian uint32
        ms.WriteByte((byte)localPubKeyRaw.Length);
        ms.Write(localPubKeyRaw);
        ms.Write(ciphertext);
        ms.Write(tag);

        return new EncryptedPayload(ms.ToArray());
    }

    private static string B64Url(byte[] data) =>
        Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static byte[] B64UrlDecode(string s)
    {
        s = s.Replace('-', '+').Replace('_', '/');
        switch (s.Length % 4)
        {
            case 2: s += "=="; break;
            case 3: s += "="; break;
        }
        return Convert.FromBase64String(s);
    }
}
