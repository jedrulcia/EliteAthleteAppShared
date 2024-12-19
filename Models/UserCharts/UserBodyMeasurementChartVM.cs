using EliteAthleteAppShared.Models.UserCharts;

namespace EliteAthleteAppShared.Models.Charts
{
    public class UserBodyMeasurementChartVM
	{
		public List<DataPointVM> ChestDataPointVMs { get; set; } = new List<DataPointVM?>();
		public List<DataPointVM> ArmsDataPointVMs { get; set; } = new List<DataPointVM?>();
		public List<DataPointVM> WaistDataPointVMs { get; set; } = new List<DataPointVM?>();
		public List<DataPointVM> ThighsDataPointVMs { get; set; } = new List<DataPointVM?>();
		public List<DataPointVM> HipsDataPointVMs { get; set; } = new List<DataPointVM?>();
	}
}
