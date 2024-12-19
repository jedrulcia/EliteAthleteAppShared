using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.TrainingModule;

namespace EliteAthleteAppShared.Contracts
{
	public interface ITrainingModuleRepository : IGenericRepository<TrainingModule>
	{
		// GETS LIST OF TRAINING MODULE INDEX VM
		Task<TrainingModuleIndexVM> GetTrainingModuleIndexVMAsync(string? userId);

		// GETS LIST OF TRAINING MODULES VIEW MODELS
		Task<List<TrainingModuleVM>> GetTrainingModuleVMsAsync(string userId);

		// GETS TRAINING MODULE CREATE VIEW MODEL
		TrainingModuleCreateVM GetTrainingModuleCreateVM(string userId, string coachId);

		// GETS TRAINING MODULE EDIT VIEW MODEL
		Task<TrainingModuleCreateVM> GetTrainingModuleEditVMAsync(int trainingModuleId);
		
		// GETS TRAINING MODULE DELETE VIEW MODEL
		Task<TrainingModuleDeleteVM> GetTrainingModuleDeleteVM(int trainingModuleId);

		// CREATES A NEW TRAINING MODULE
		Task CreateTrainingModuleAsync(TrainingModuleCreateVM trainingModuleCreateVM);

		// EDITS EXSITING TRAINING MODULE
		Task EditTrainingModuleAsync(TrainingModuleCreateVM trainingModuleCreateVM);

		// DELETES THE TRAINING MODULE AND ALL ASSOCIATED TRAINING PLANS
		Task DeleteTrainingModuleAsync(int id);

	}
}
