using Common.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppRecipe.Controllers
{

    public class BackofficeController : Controller
    {     
        public ActionResult Backoffice()
        {
            ViewBag.Name = User.Identity.Name;
            return View();
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
            return RedirectToAction("Backoffice", "Backoffice");
        }               

    }
}
