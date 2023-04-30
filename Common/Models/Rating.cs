namespace Common.Models
{    public class Rating
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public string RatingName { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public Rating() { }

        public Rating(int id, string ratingName, string comment, string userName, string userEmail)
        {
            Id = id;
            RatingName = ratingName;
            Comment = comment;
            UserName = userName;
            UserEmail = userEmail;
        }
    }
}
