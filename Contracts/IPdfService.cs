using EliteAthleteAppShared.Repositories;

namespace EliteAthleteAppShared.Contracts
{
    public interface IPdfService
    {
        // GENERATES TRAINING MODULE IN PDF
        Task<byte[]> GetTrainingModulePDFAsync(int trainingModuleId);
    }
}
