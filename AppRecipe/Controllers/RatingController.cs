using Common.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AppRecipe.Controllers
{
    public class RatingController : Controller
    {
        public IActionResult NewRating()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewRating(string name, string email, string comment, string rating)
        {
            RatingRepository ratingRepository = new();
            if (!ModelState.IsValid)
            {
                return View();
            }
            int userId = ratingRepository.GetUserIdByEmail(email);
            if (userId == 0)
            {               
                return View();
            }
           
            ratingRepository.CreateRating(userId, comment, rating);

            return RedirectToAction("Index", "Home");
        }
        public ActionResult ListRating()
        {
            ViewBag.Name = User.Identity.Name;
            RatingRepository ratingRepository = new();
            List<Rating> ratings = ratingRepository.GetAllRating();
            return View(ratings);
        }
        public IActionResult EditRating(int id)
        {
            RatingRepository ratingRepository = new();
            var rating = ratingRepository.GetRatingById(id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRating(Rating rating)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    RatingRepository ratingRepository = new();
                    ratingRepository.UpdateRating(rating);
                    return RedirectToAction("ListRating");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the user.");
                    return View(rating);
                }
            }
            else
            {
                return View(rating);
            }
        }

        public IActionResult DeleteRating(int id)
        {
            RatingRepository ratingRepository = new();
            var rating = ratingRepository.GetRatingById(id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            RatingRepository ratingRepository = new();
            ratingRepository.Delete(id);
            return RedirectToAction("ListRating");
        }







    }
}
