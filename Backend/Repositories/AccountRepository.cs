using Microsoft.AspNetCore.Identity;
using NoteApp.Models;
using System.Threading.Tasks;

namespace NoteApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            if (model.Password == null)
            {
                throw new ArgumentNullException(nameof(model.Password), "Password cannot be null");
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            if (model.Email == null)
            {
                throw new ArgumentNullException(nameof(model.Email), "Email cannot be null");
            }

            if (model.Password == null)
            {
                throw new ArgumentNullException(nameof(model.Password), "Password cannot be null");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
