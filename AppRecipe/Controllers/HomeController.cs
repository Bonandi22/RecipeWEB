using Common.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AppRecipe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {           
                   
            ViewBag.Name = User.Identity.Name;
            List<Recipe> recipes = new RecipeRepository().GetRecipes();
            return View(recipes);
        }
        public IActionResult AboutUs()
        {
            ViewBag.Name = User.Identity.Name;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Name = User.Identity.Name;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
