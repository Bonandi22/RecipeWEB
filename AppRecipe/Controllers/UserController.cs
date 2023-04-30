using Common.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AppRecipe.Controllers
{
    public class UserController : Controller
    {
        public ActionResult ListUsers()
        {
            ViewBag.Name = User.Identity.Name;
            UserRepository userRepository = new();
            List<User> users = userRepository.GetAllUsers();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            UserRepository userRepository = new();

            if (!ModelState.IsValid)
            {
                return View(user);
            }
            try
            {
                userRepository.CreateUser(user);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult EmailExists()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EmailExists(User user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("", "Email are required");
                return View();
            }
            UserRepository userRepository = new();

            // Check if the email is registered in the database        

            if (!userRepository.EmailExists(user))
            {
                ModelState.AddModelError("Email", "Invalid email or password");
                return View(user);
            }

            return RedirectToAction("UpdatePassword", new { email = user.Email });
        }
        public IActionResult UpdatePassword(string email)
        {
            User user = new User { Email = email };
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePassword(User user)
        {
            if (string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError("", "Password is required");
                return View(user);
            }

            UserRepository userRepository = new();

            // update the user's password
            userRepository.UptadePass(user.Email, user.Password);

            return RedirectToAction("Login", "Login");
        }              
        public IActionResult EditUser(int id)
        {
            UserRepository userRepository = new();
            var user = userRepository.GetUserById(id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(User user)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    UserRepository userRepository = new();
                    userRepository.UpdateUser(user);
                    return RedirectToAction("ListUsers");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the user.");
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }
        }

        public IActionResult DeleteUser(int id)
        {
            UserRepository userRepository = new();
            User user = userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            UserRepository userRepository = new();
            userRepository.Delete(id);
            return RedirectToAction("ListUsers");
        }


    }
}
