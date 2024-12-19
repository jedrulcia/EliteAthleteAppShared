using System.ComponentModel.DataAnnotations;

namespace EliteAthleteAppShared.Models.TrainingExercise
{
	public class TrainingExerciseMuscleGroupVM
	{
		// IDs
		public int Id { get; set; }

		// STRINGS

		[Required]
		[Display(Name = "Muscle Group")]
		public string Name { get; set; }
	}
}
