﻿using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.TrainingExercise;

namespace EliteAthleteAppShared.Contracts
{
	public interface ITrainingExerciseRepository : IGenericRepository<TrainingExercise>
	{
		// GETS EXERCISE INDEX VIEW MODEL (COACH ID)
		Task<TrainingExerciseIndexVM> GetExerciseIndexVMAsync(int? exerciseMediaId);

		// GETS LIST OF PUBLIC OR PRIVATE EXERCISES
		Task<List<TrainingExerciseVM>> GetExerciseVMsAsync(string? coachId);

		// GETS LIST OF PRIVATE EXERCISES SET TO PUBLIC
		Task<List<TrainingExerciseVM>> GetExerciseAdminVMsAsync();

		// GETS SET EXERCISE AS PUBLIC VIEW MODEL
		Task<TrainingExerciseAsPublicVM> GetExerciseAsPublicVMAsync(int? trainingExerciseId);

		// GETS EXERCISE CREATE VIEW MODEL
		Task<TrainingExerciseCreateVM> GetExerciseCreateVMAsync(int? privateExerciseCount);

		// GETS EXERCISE DELETE VIEW MODEL
		Task<TrainingExerciseDeleteVM> GetExerciseDeleteVMAsync(int id);

		// GETS EXERCISE DETAILS VIEW MODEL
		Task<TrainingExerciseVM?> GetExerciseDetailsVMAsync(int id);

		// GETS EXERCISE EDIT VIEW MODEL
		Task<TrainingExerciseCreateVM> GetExerciseEditVMAsync(int id);

		// CREATES A NEW DATABASE ENTITY IN THE EXERCISE TABLE
		Task CreateExerciseAsync(TrainingExerciseCreateVM exerciseCreateVM);

		// EDITS EXISTING DATABAASE ENTITY IN THE EXERCISE TABLE
		Task EditExerciseAsync(TrainingExerciseCreateVM exerciseCreateVM);

		// DELETES EXISTING DATABASE ENTITY FROM THE EXERCISE TABLE
		Task DeleteExerciseAsync(TrainingExerciseDeleteVM trainingExerciseDeleteVM);

		// SETS EXERCISE AS PUBLIC
		Task SetExerciseAsPublic(int trainingExerciseId);
	}
}
