using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GymBudgetApp.Pages
{
    public class SignOutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public SignOutModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Identity/Account/Login");
        }
    }
}
