﻿using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Contracts;

namespace EliteAthleteAppShared.Repositories
{
    public class TrainingExerciseMuscleGroupRepository : GenericRepository<TrainingExerciseMuscleGroup>, ITrainingExerciseMuscleGroupRepository
	{
		private readonly ApplicationDbContext context;

		public TrainingExerciseMuscleGroupRepository(ApplicationDbContext context) : base(context)
		{
			this.context = context;
		}
	}
}
