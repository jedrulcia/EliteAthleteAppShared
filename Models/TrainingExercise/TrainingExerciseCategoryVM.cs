using System.ComponentModel.DataAnnotations;

namespace EliteAthleteAppShared.Models.TrainingExercise
{
    public class TrainingExerciseCategoryVM
    {
        // IDs
        public int Id { get; set; }

        // STRINGS

        [Display(Name="Category")]
        public string Name { get; set; }
    }
}
