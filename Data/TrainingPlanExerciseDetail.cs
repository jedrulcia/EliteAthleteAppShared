
namespace EliteAthleteAppShared.Data
{
	public class TrainingPlanExerciseDetail
	{
		// IDs
		public int? Id { get; set; }
		public int? ExerciseId { get; set; }
		public int? TrainingPlanPhaseId { get; set; }

		// STRINGS
		public string? Index { get; set; }
		public string? Sets { get; set; }
		public string? Units { get; set; }
		public string? Weight { get; set; }
		public string? RestTime { get; set; }
		public string? Note { get; set; }
	}
}
