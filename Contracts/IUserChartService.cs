using EliteAthleteAppShared.Models.Charts;
using EliteAthleteAppShared.Services;

namespace EliteAthleteAppShared.Contracts
{
	public interface IUserChartService
	{
		Task<UserChartsVM> GetUserCharts(string? userId);
	}
}
