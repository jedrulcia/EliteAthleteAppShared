﻿using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.UserMedicalTest;
using Microsoft.AspNetCore.Http;

namespace EliteAthleteAppShared.Contracts
{
    public interface IUserMedicalTestRepository : IGenericRepository<UserMedicalTest>
    {
        // GETS LIST OF USER BODY ANALYSIS
        Task<List<UserMedicalTestVM>> GetUserMedicalTestVMsAsync(string userId);

        // GETS USER BODY ANALYSIS CREATE VM
        Task<UserMedicalTestCreateVM> GetUserMedicalTestCreateVM(string userId);

        // GETS USER BODY ANALYSIS EDIT VIEW MODEL
        Task<UserMedicalTestCreateVM> GeUserMedicalTestEditVM(int medicalTestId);

        // GETS USER BODY ANALYSIS DELETE VIEW MODEL
        Task<UserMedicalTestDeleteVM> GetUserMedicalTestDeleteVM(int medicalTestId);

        // CREATES NEW USER BODY ANALYSIS ENTITY
        Task CreateUserMedicalTestAsync(UserMedicalTestCreateVM userMedicalTestCreateVM, IFormFile? file);

        // EDITS EXSITING USER BODY ANALYSIS ENTITY
        Task EditUserMedicalTestAsync(UserMedicalTestCreateVM userMedicalTestCreateVM, IFormFile? file);

        // DELETES USER BODY ANALYSIS ENTITY
        Task DeleteUserMedicalTestAsync(UserMedicalTestDeleteVM userMedicalTestDeleteVM);
    }
}
