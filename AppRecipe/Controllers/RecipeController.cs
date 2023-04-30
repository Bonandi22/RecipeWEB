using Common.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace AppRecipe.Controllers
{
    public class RecipeController : Controller
    {
        public ActionResult ListRecipe()
        {
            ViewBag.Name = User.Identity.Name;
            RecipeRepository recipeRepository = new();
            List<Recipe> recipes = recipeRepository.GetRecipes();
            return View(recipes);
        }
        public IActionResult NewRecipe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewRecipe(Recipe recipe)
        {
            RecipeRepository recipeRepositories = new();

            if (!ModelState.IsValid)
            {
                return View(recipe);
            }
            recipeRepositories.CreateRecipe(recipe);

            return RedirectToAction("ListRecipe", "Recipe");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Newingredient(Ingredient ingredient)
        //{
        //    RecipeRepository recipeRepositories = new();

        //    if (!ModelState.IsValid)
        //    {
        //        return View(ingredient);
        //    }
        //    recipeRepositories.CreateIngredient(ingredient);

        //    return RedirectToAction("ListRecipe", "Recipe");
        //}

        public IActionResult EditRecipe(int id)
        {
            RecipeRepository recipeRepository = new();
            var recipe = recipeRepository.GetRecipeById(id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRecipe(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    RecipeRepository recipeRepository = new();
                    recipeRepository.UpdateRecipe(recipe);
                    return RedirectToAction("ListRecipe");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the user.");
                    return View(recipe);
                }
            }
            else
            {
                return View(recipe);
            }
        }
        public IActionResult DeleteRecipe(int id)
        {
            RecipeRepository recipeRepository = new();
            var recipe = recipeRepository.GetRecipeById(id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            RecipeRepository recipeRepository = new();
            recipeRepository.Delete(id);
            return RedirectToAction("ListRecipe");
        }


    }

}
