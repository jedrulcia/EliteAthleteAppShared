﻿using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Contracts;

namespace EliteAthleteAppShared.Repositories
{
    public class TrainingPlanPhaseRepository : GenericRepository<TrainingPlanPhase>, ITrainingPlanPhaseRepository
	{
		private readonly ApplicationDbContext context;

		public TrainingPlanPhaseRepository(ApplicationDbContext context) : base(context)
		{
			this.context = context;
		}
	}
}
