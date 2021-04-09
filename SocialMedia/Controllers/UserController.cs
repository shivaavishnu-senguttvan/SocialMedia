using System.Security.Claims;
using System.Threading.Tasks;
using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Infrastructure;
using SocialMedia.ViewModels;

namespace SocialMedia.Controllers
{
    public class UserController : Controller
    {
        private static string ApiKey = "AIzaSyBkavSoz-XInWIz9ML3LvY6Sj4PvAb-t1Y";
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new AppUser 
                { 
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager
                    .CreateAsync(user, model.Password);

                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }

            return View(model);
        } 
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var ab = await auth.SignInWithEmailAndPasswordAsync(model.Email, model.Password);
                string token = ab.FirebaseToken;
                var user = ab.User;
                if (token != "")
                {
                    ClaimsPrincipal claimsPrinciple = new ClaimsPrincipal();
                    _signInManager.IsSignedIn(claimsPrinciple);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            
            return RedirectToAction(nameof(Login));
        }
    }
}