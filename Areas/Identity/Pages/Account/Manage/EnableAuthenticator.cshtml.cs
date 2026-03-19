using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GymBudgetApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class EnableAuthenticatorModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public EnableAuthenticatorModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string SharedKey { get; set; } = string.Empty;
        public bool Is2faEnabled { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            [Display(Name = "Verification Code")]
            public string Code { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            if (!Is2faEnabled)
            {
                await LoadSharedKeyAsync(user);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAsync(user);
                return Page();
            }

            var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                _userManager.Options.Tokens.AuthenticatorTokenProvider,
                verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Input.Code", "Verification code is invalid.");
                await LoadSharedKeyAsync(user);
                return Page();
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            StatusMessage = "Two-factor authentication has been enabled successfully.";
            Is2faEnabled = true;

            return RedirectToPage();
        }

        private async Task LoadSharedKeyAsync(IdentityUser user)
        {
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            SharedKey = FormatKey(unformattedKey!);
        }

        private static string FormatKey(string unformattedKey)
        {
            var result = new System.Text.StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition));
            }
            return result.ToString().ToLowerInvariant();
        }
    }
}
