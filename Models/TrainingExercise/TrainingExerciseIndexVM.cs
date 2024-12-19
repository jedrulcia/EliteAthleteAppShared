using System.ComponentModel.DataAnnotations;

namespace EliteAthleteAppShared.Models.TrainingExercise
{
    public class TrainingExerciseIndexVM
	{
        // IDs
		public string CoachId { get; set; }
        public int? ExerciseMediaId { get; set; }

        // VALIDATION
        public int? PrivateExerciseCount { get; set; }
    }
}
