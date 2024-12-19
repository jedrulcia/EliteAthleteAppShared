using AutoMapper;
using Microsoft.AspNetCore.Identity;
using EliteAthleteAppShared.Contracts;
using EliteAthleteAppShared.Data;

namespace EliteAthleteAppShared.Repositories
{
	public class TrainingPlanExerciseDetailRepository : GenericRepository<TrainingPlanExerciseDetail>, ITrainingPlanExerciseDetailRepository
	{
		private readonly ApplicationDbContext context;

		public TrainingPlanExerciseDetailRepository(ApplicationDbContext context) : base(context)
		{
			this.context = context;
		}
	}
}
