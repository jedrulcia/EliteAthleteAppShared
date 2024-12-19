using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.TrainingExercise;
using Microsoft.AspNetCore.Http;

namespace EliteAthleteAppShared.Contracts
{
    public interface ITrainingExerciseMediaRepository : IGenericRepository<TrainingExerciseMedia>
    {
        // GETS TRAINING EXERCISE MEDIA CREATE VIEW MODEL
        Task<TrainingExerciseMediaCreateVM> GetTrainingExerciseMediaCreateVMAsync(int exerciseMediaId);

        // UPLOADS IMAGE TO AZURE BLOB STORAGE AND SAVES URL IN EXERCISE MEDIA ENTITY
        Task UploadImageAsync(int id, int index, IFormFile imageFile);

        // UPLOADS VIDEO TO AZURE BLOB STORAGE AND SAVES URL IN EXERCISE MEDIA ENTITY
        Task UploadVideoAsync(int id, IFormFile videoFile);

        // REMOVES IMAGE FROM AZURE BLOB STORAGE AND URL FROM EXERCISE MEDIA ENTITY
        Task DeleteImageAsync(int id, int index);

        // REMOVES VIDEO FROM AZURE BLOB STORAGE AND URL FROM EXERCISE MEDIA ENTITY
        Task DeleteVideoAsync(int id);

        // REMOVES URL OF DELETED EXERCISE AND EXERCISE MEDIA ENTITY
        Task DeleteExerciseMediaAsync(int? trainingExerciseMediaId);
    }
}
