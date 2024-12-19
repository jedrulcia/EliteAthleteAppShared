using System.ComponentModel.DataAnnotations;

namespace EliteAthleteAppShared.Models.User
{
	public class UserInfoVM
	{
		// OBJECTS
		public UserVM? CoachVM { get; set; }
		public UserVM? NewCoachVM { get; set; }
		public UserVM? UserVM { get; set; }
	}
}
