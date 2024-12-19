﻿using EliteAthleteAppShared.Models.UserCharts;

namespace EliteAthleteAppShared.Models.Charts
{
    public class UserBodyAnalysisChartVM
	{
		public List<DataPointVM> WeightDataPointVMs { get; set; } = new List<DataPointVM?>();
		public List<DataPointVM> FatPercentageDataPointVMs { get; set; } = new List<DataPointVM?>();
		public List<DataPointVM> MusclePercentageDataPointVMs { get; set; } = new List<DataPointVM?>();
		public List<DataPointVM> WaterPercentageDataPointVMs { get; set; } = new List<DataPointVM?>();
	}
}
