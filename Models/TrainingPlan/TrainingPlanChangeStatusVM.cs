using System.ComponentModel.DataAnnotations;

namespace EliteAthleteAppShared.Models.TrainingPlan
{
	public class TrainingPlanChangeStatusVM
	{
		// IDs
		public int? Id { get; set; }
		public int? TrainingModuleId { get; set; }

		// STRINGS

		[Display(Name = "Raport")]
		public string? Raport {  get; set; }
	}
}
