using GymBudgetApp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<GymBudgetApp.Services.NotesPanelState>();
builder.Services.AddScoped<GymBudgetApp.Services.PermissionService>();
builder.Services.AddScoped<GymBudgetApp.Services.NotificationService>();
builder.Services.AddScoped<GymBudgetApp.Services.ChatService>();
builder.Services.AddScoped<GymBudgetApp.Services.AuditService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<GymBudgetApp.Services.PushNotificationService>();
builder.Services.AddSingleton<GymBudgetApp.Services.BackupService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<GymBudgetApp.Services.BackupService>());

var dbFolder = Environment.GetEnvironmentVariable("DB_PATH")
    ?? Directory.GetCurrentDirectory();
var dbPath = Path.Combine(dbFolder, "gymbudget.db");

// Persist Data Protection keys so auth cookies survive redeployments
var keysFolder = Path.Combine(dbFolder, "keys");
Directory.CreateDirectory(keysFolder);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("GymBudgetApp");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath};Foreign Keys=True"));
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath};Foreign Keys=True"), ServiceLifetime.Scoped);

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Password policy
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;

    // Lockout after 5 failed attempts for 15 minutes
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

// Session timeout — expire after 60 minutes of inactivity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = "/Identity/Account/Login";
});

// Stripe configuration
var stripeSecretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY")
    ?? builder.Configuration["Stripe:SecretKey"] ?? "";
if (!string.IsNullOrEmpty(stripeSecretKey))
    Stripe.StripeConfiguration.ApiKey = stripeSecretKey;

builder.Services.AddTransient<IEmailSender, GymBudgetApp.Services.EmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    var disableAutoMigrations = string.Equals(
        Environment.GetEnvironmentVariable("DisableAutoMigrations"),
        "true",
        StringComparison.OrdinalIgnoreCase);
    var clearMigrationLock = string.Equals(
        Environment.GetEnvironmentVariable("ClearMigrationLock"),
        "true",
        StringComparison.OrdinalIgnoreCase);

    await using (var connection = new SqliteConnection($"Data Source={dbPath}"))
    {
        await connection.OpenAsync();

        if (clearMigrationLock)
        {
            await using var clearCommand = connection.CreateCommand();
            clearCommand.CommandText = """
                SELECT COUNT(*)
                FROM sqlite_master
                WHERE type = 'table' AND name = '__EFMigrationsLock';
                """;
            var hasMigrationLockTable = Convert.ToInt32(await clearCommand.ExecuteScalarAsync()) > 0;

            if (hasMigrationLockTable)
            {
                clearCommand.CommandText = "DELETE FROM \"__EFMigrationsLock\";";
                var clearedRows = await clearCommand.ExecuteNonQueryAsync();
                logger.LogWarning("Cleared {ClearedRows} rows from __EFMigrationsLock.", clearedRows);
            }
            else
            {
                logger.LogInformation("__EFMigrationsLock table not present; nothing to clear.");
            }
        }
    }

    if (disableAutoMigrations)
    {
        logger.LogWarning("Automatic database migrations are disabled for this startup.");
    }
    else
    {
        db.Database.Migrate();
    }

    await using (var connection = new SqliteConnection($"Data Source={dbPath};Foreign Keys=True"))
    {
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT COUNT(*)
            FROM pragma_table_info('Payments')
            WHERE name = 'SeasonId';
            """;
        var hasSeasonId = Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
        logger.LogInformation("Payments.SeasonId column present: {HasSeasonId}", hasSeasonId);

        command.CommandText = "PRAGMA foreign_keys;";
        var foreignKeysEnabled = Convert.ToInt32(await command.ExecuteScalarAsync()) == 1;
        logger.LogInformation("SQLite foreign key enforcement enabled: {ForeignKeysEnabled}", foreignKeysEnabled);

        command.CommandText = "SELECT COUNT(*) FROM pragma_foreign_key_check;";
        var foreignKeyViolations = Convert.ToInt32(await command.ExecuteScalarAsync());
        logger.LogInformation("SQLite foreign key violations detected at startup: {ForeignKeyViolations}", foreignKeyViolations);
    }

    // Ensure roles exist and assign roles to existing users without one
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    foreach (var roleName in new[] { "Parent", "Employee" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    var adminEmail = app.Configuration["AdminEmail"] ?? "deshaun@tntgym.org";
    var allUsers = userManager.Users.ToList();
    foreach (var user in allUsers)
    {
        var roles = await userManager.GetRolesAsync(user);
        if (!roles.Any())
        {
            var isAdmin = string.Equals(user.Email, adminEmail, StringComparison.OrdinalIgnoreCase);
            await userManager.AddToRoleAsync(user, isAdmin ? "Employee" : "Parent");
        }
    }

    // Data migration completed - no longer needed
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();

// Stripe webhook handler
async Task<IResult> HandleStripeWebhook(HttpContext context, AppDbContext db)
{
    var webhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET")
        ?? app.Configuration["Stripe:WebhookSecret"] ?? "";
    var json = await new StreamReader(context.Request.Body).ReadToEndAsync();

    try
    {
        var stripeEvent = Stripe.EventUtility.ConstructEvent(json,
            context.Request.Headers["Stripe-Signature"], webhookSecret);

        if (stripeEvent.Type == "checkout.session.completed")
        {
            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
            if (session != null)
            {
                var payment = db.Set<GymBudgetApp.Models.Payment>()
                    .FirstOrDefault(p => p.StripeSessionId == session.Id);
                if (payment != null && payment.Status != GymBudgetApp.Models.PaymentStatus.Paid)
                {
                    payment.Status = GymBudgetApp.Models.PaymentStatus.Paid;
                    payment.StripePaymentIntentId = session.PaymentIntentId;
                    payment.PaidAt = DateTime.UtcNow;
                    await db.SaveChangesAsync();

                    // Send payment notification
                    var athlete = await db.Athletes.FindAsync(payment.AthleteId);
                    if (athlete != null)
                    {
                        using var scope = app.Services.CreateScope();
                        var notifier = scope.ServiceProvider.GetRequiredService<GymBudgetApp.Services.NotificationService>();
                        await notifier.NotifyPayment(athlete.Name, payment.Amount, 0);
                    }
                }
            }
        }
        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest();
    }
}

app.MapPost("/api/stripe-webhook", HandleStripeWebhook).AllowAnonymous();
app.MapPost("/stripe-webhook", HandleStripeWebhook).AllowAnonymous();

// Admin backup download
app.MapGet("/api/backup/download", async (HttpContext context) =>
{
    var adminEmail = app.Configuration["AdminEmail"] ?? "deshaun@tntgym.org";
    var user = context.User;
    var email = user.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                ?? user.FindFirst("email")?.Value;
    if (!string.Equals(email, adminEmail, StringComparison.OrdinalIgnoreCase))
        return Results.Forbid();

    var backupService = app.Services.GetRequiredService<GymBudgetApp.Services.BackupService>();
    var dbPath = backupService.GetDbPath();
    if (!File.Exists(dbPath)) return Results.NotFound();

    var bytes = await File.ReadAllBytesAsync(dbPath);
    return Results.File(bytes, "application/octet-stream", $"gymbudget-backup-{DateTime.UtcNow:yyyy-MM-dd}.db");
}).RequireAuthorization();

// Admin trigger manual backup
app.MapPost("/api/backup/create", (HttpContext context) =>
{
    var adminEmail = app.Configuration["AdminEmail"] ?? "deshaun@tntgym.org";
    var email = context.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                ?? context.User.FindFirst("email")?.Value;
    if (!string.Equals(email, adminEmail, StringComparison.OrdinalIgnoreCase))
        return Results.Forbid();

    var backupService = app.Services.GetRequiredService<GymBudgetApp.Services.BackupService>();
    backupService.CreateBackup();
    return Results.Ok("Backup created");
}).RequireAuthorization();

// Push notification subscribe/unsubscribe
app.MapPost("/api/push/subscribe", async (HttpContext context) =>
{
    var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    if (userId == null) return Results.Unauthorized();

    var body = await context.Request.ReadFromJsonAsync<PushSubscribeRequest>();
    if (body == null || string.IsNullOrEmpty(body.Endpoint)) return Results.BadRequest();

    var dbFactory = context.RequestServices.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var db = await dbFactory.CreateDbContextAsync();

    var existing = await db.PushSubscriptions.FirstOrDefaultAsync(
        s => s.UserId == userId && s.Endpoint == body.Endpoint);
    if (existing == null)
    {
        db.PushSubscriptions.Add(new GymBudgetApp.Models.PushSubscriptionRecord
        {
            UserId = userId,
            Endpoint = body.Endpoint,
            P256dh = body.P256dh ?? "",
            Auth = body.Auth ?? ""
        });
        await db.SaveChangesAsync();
    }
    return Results.Ok();
}).RequireAuthorization();

app.MapPost("/api/push/unsubscribe", async (HttpContext context) =>
{
    var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    if (userId == null) return Results.Unauthorized();

    var body = await context.Request.ReadFromJsonAsync<PushSubscribeRequest>();
    if (body == null) return Results.BadRequest();

    var dbFactory = context.RequestServices.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var db = await dbFactory.CreateDbContextAsync();

    var sub = await db.PushSubscriptions.FirstOrDefaultAsync(
        s => s.UserId == userId && s.Endpoint == body.Endpoint);
    if (sub != null)
    {
        db.PushSubscriptions.Remove(sub);
        await db.SaveChangesAsync();
    }
    return Results.Ok();
}).RequireAuthorization();

app.MapGet("/api/push/vapid-key", (HttpContext context) =>
{
    var config = context.RequestServices.GetRequiredService<IConfiguration>();
    var key = config["VAPID:PublicKey"] ?? config["VAPID_PUBLIC_KEY"] ?? "";
    return Results.Ok(new { key });
});

app.MapFallbackToPage("/_Host");

app.Run();

record PushSubscribeRequest(string? Endpoint, string? P256dh, string? Auth);
