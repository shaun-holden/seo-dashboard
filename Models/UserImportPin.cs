using System.Security.Cryptography;
using System.Text;

namespace GymBudgetApp.Models
{
    public class UserImportPin
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public string PinHash { get; set; } = string.Empty;

        public static string HashPin(string pin)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(pin));
            return Convert.ToBase64String(bytes);
        }

        public bool VerifyPin(string enteredPin)
        {
            // Support both hashed and legacy plaintext PINs
            if (!string.IsNullOrEmpty(PinHash))
                return PinHash == HashPin(enteredPin);
            return Pin == enteredPin;
        }
    }
}
