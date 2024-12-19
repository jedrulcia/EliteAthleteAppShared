using EliteAthleteAppShared.Models.Charts;
using EliteAthleteAppShared.Models.TrainingPlan;
using EliteAthleteAppShared.Models.User;

namespace EliteAthleteAppShared.Models.Home
{
	public class HomeIndexVM
	{
		public UserChartsVM? UserChartsVM { get; set; }
		public bool IsLoggedIn { get; set; } = false;
		public UserVM? UserVM { get; set; }
		public TrainingPlanDetailsVM? TrainingPlanDetailsVM { get; set; }
	}
}
