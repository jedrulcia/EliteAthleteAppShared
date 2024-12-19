﻿using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.Charts;
using EliteAthleteAppShared.Models.UserBodyMeasurements;

namespace EliteAthleteAppShared.Contracts
{
    public interface IUserBodyMeasurementsRepository : IGenericRepository<UserBodyMeasurements>
    {
        // GETS LIST OF USER BODY MEASUREMENTS
        Task<List<UserBodyMeasurementsVM>> GetUserBodyMeasurementsVMsAsync(string userId);

        // GETS USER BODY MEASUREMENTS CREATE VM
        Task<UserBodyMeasurementsCreateVM> GetUserBodyMeasurementCreateVM(string userId);

        // GETS USER BODY MEASUREMENTS EDIT VIEW MODEL
        Task<UserBodyMeasurementsCreateVM> GeUserBodyMeasurementEditVM(int bodyMeasurementId);

        // GETS USER BODY MEASUREMENTS DELETE VIEW MODEL
        Task<UserBodyMeasurementsDeleteVM> GetUserBodyMeasurementDeleteVM(int bodyMeasurementId);

		// GETS USER BODY MEASUREMENTS CHART
		Task<UserBodyMeasurementChartVM> GetUserBodyMeasurementsChartVMAsync(string userId);

		// CREATES NEW USER BODY MEASUREMENTS ENTITY
		Task CreateUserBodyMeasurementAsync(UserBodyMeasurementsCreateVM userBodyMeasurementCreateVM);

        // EDITS EXSITING USER BODY MEASUREMENTS ENTITY
        Task EditUserBodyMeasurementAsync(UserBodyMeasurementsCreateVM userBodyMeasurementCreateVM);

        // DELETES USER BODY MEASUREMENTS ENTITY
        Task DeleteUserBodyMeasurementAsync(UserBodyMeasurementsDeleteVM userBodyMeasurementDeleteVM);
    }
}
