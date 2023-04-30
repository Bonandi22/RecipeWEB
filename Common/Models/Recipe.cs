namespace Common.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DificultId { get; set; }
        public int CategaryId { get; set; }
        public string PrepationMethod { get; set; }
        public string PreparationTime { get; set; }
     

        public Recipe()
        {
        }
        public Recipe(int recipeId, string recipeName, string description, string prepationMethod, string preparationTime)
        {
            Id = recipeId;
            Name = recipeName;
            Description = description;
            PrepationMethod = prepationMethod;
            PreparationTime = preparationTime;
        }
    }

}
