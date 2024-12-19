﻿using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Contracts;

namespace EliteAthleteAppShared.Repositories
{
    public class TrainingExerciseCategoryRepository : GenericRepository<TrainingExerciseCategory>, ITrainingExerciseCategoryRepository
	{
		private readonly ApplicationDbContext context;

		public TrainingExerciseCategoryRepository(ApplicationDbContext context) : base(context)
		{
			this.context = context;
		}
	}
}
