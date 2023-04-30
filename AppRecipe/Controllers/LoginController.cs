using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Common.Models;
using Database.Repositories;

namespace AppRecipe.Controllers
{

    public class LoginController : Controller
    {       
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            UserRepository userRepository = new();

            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "Email and password are required");
                return View();
            }

            if (userRepository.Login(model))
            {
                var name = userRepository.GetNameByEmail(model.Email);
                var adm = userRepository.GetADMByEmail(model.Email);
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, name, adm)                    
                    };

                var claimsIdentity = new ClaimsIdentity(
                   claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity));

                //Store the user name in the ViewBag
                ViewBag.Name = name;
                ViewBag.adm = adm;               

                // Redirect to the home page if the login is successful
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Set the login failed flag to true and pass the error message to the ViewBag
                ViewBag.ErrorMessage = "Login failed. Please check your email and password.";
                return View();
            }
        }

        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
