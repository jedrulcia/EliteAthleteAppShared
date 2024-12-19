﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.TrainingPlan;

namespace EliteAthleteAppShared.Contracts
{
	public interface ITrainingPlanRepository : IGenericRepository<TrainingPlan>
	{
		// GETS A LIST OF SPECIFIC USER TRAINING PLANS BASED ON PROVIDED TRAINING PLAN IDs.
		Task<TrainingPlanIndexVM> GetTrainingPlanIndexVMAsync(int trainingModuleId);

		// GETS TRAINING PLAN LIST VM
		Task<TrainingPlanListVM> GetTrainingPlanListVMAsync(List<int> trainingPlanIds);

		// GETS THE TRAINING PLAN DETAILS VIEW MODEL FOR THE SPECIFIED TRAINING PLAN.
		Task<TrainingPlanDetailsVM> GetTrainingPlanDetailsVMAsync(int trainingPlanId);

		// GETS THE TRAINING PLAN COPY VM
		Task<TrainingPlanCopyVM> GetTrainingPlanCopyVMAsync(int? copyFromId, List<int> trainingPlanIds, int trainingModuleId);

		// GETS THE TRAINING PLAN CHANGE STATUS VM
		Task<TrainingPlanChangeStatusVM> GetTrainingPlanChangeStatusVMAsync(int trainingPlanId);

		// GET THE TRAINING PLAN ADD EXERCISE VM
		Task<TrainingPlanAddExerciseVM> GetTrainingPlanAddExerciseVMAsync(int trainingPlanId, string coachId);

		// GET THE TRAINING PLAN ADD EXERCISE VM
		Task<TrainingPlanAddExerciseVM> GetTrainingPlanEditExerciseVMAsync(int trainingPlanId, string coachId, int trainingPlanExerciseDetailId);

		// GET THE TRAINING PLAN REMOVE EXERCISE VM
		Task<TrainingPlanRemoveExerciseVM> GetTrainingPlanRemoveExerciseVM(int trainingPlanId, int trainingPlanExerciseDetailId, string name);

		// GET THE TRAINING PLAN FOR TODAY
		Task<TrainingPlanDetailsVM?> GetDailyTrainingPlanVMAsync(string userId);

		// CREATES A NEW DATABASE ENTITY IN THE TRAINING PLAN TABLE AND RETURNS THE NEW ID.
		Task<int> CreateTrainingPlanAsync(TrainingPlanCreateVM model);

		// DELETES TRAINING PLAN AND ITS EXERCISE DETAILS
		Task DeleteTrainingPlanAndDetailsAsync(int trainingPlanId);

		// ADDS AN EXERCISE TO THE SPECIFIED TRAINING PLAN.
		Task<TrainingPlanDetailsVM> AddExerciseToTrainingPlanAsync(TrainingPlanAddExerciseVM trainingPlanCreateVM);

		// EDIT AN EXERCISE IN SPECIFIED TRAINING PLAN.
		Task<TrainingPlanDetailsVM> EditExerciseInTrainingPlanAsync(TrainingPlanAddExerciseVM trainingPlanCreateVM);

		// REMOVES AN EXERCISE FROM THE SPECIFIED TRAINING PLAN BASED ON TRAINING PLAN ID AND EXERCISE INDEX.
		Task RemoveExerciseFromTrainingPlanAsync(TrainingPlanRemoveExerciseVM trainingPlanRemoveExerciseVM);

		// CHANGES THE STATUS OF THE TRAINING PLAN (ACTIVE/NOT ACTIVE).
		Task CompleteTrainingPlanAsync(int trainingPlanId, string raport);

		// COPIES A TRAINING PLAN TO ANOTHER TRAINING PLAN WITHIN THE SAME MODULE.
		Task CopyTrainingPlanAsync(int copyFromId, int copyToId);
	}
}
