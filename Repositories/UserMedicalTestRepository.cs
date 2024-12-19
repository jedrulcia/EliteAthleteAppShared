﻿using AutoMapper;
using EliteAthleteAppShared.Contracts;
using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.TrainingOrm;
using EliteAthleteAppShared.Models.UserBodyAnalysis;
using EliteAthleteAppShared.Models.UserMedicalTest;
using Microsoft.AspNetCore.Http;

namespace EliteAthleteAppShared.Repositories
{
    public class UserMedicalTestRepository : GenericRepository<UserMedicalTest>, IUserMedicalTestRepository
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;
		private readonly IGoogleDriveService googleDriveService;

		public UserMedicalTestRepository(ApplicationDbContext context, IMapper mapper, IGoogleDriveService googleDriveService) : base(context)
		{
			this.context = context;
			this.mapper = mapper;
			this.googleDriveService = googleDriveService;
		}

		// GETS LIST OF USER BODY MEASUREMENTS
		public async Task<List<UserMedicalTestVM>> GetUserMedicalTestVMsAsync(string userId)
		{
			return mapper.Map<List<UserMedicalTestVM>>((await GetAllAsync()).Where(tm => tm.UserId == userId));
		}

		// GETS USER BODY MEASUREMENTS CREATE VM
		public async Task<UserMedicalTestCreateVM> GetUserMedicalTestCreateVM(string userId)
		{
			var formattedDate = DateTime.Now.ToString("yyyy-MM-dd");
			var userMt = (await GetAllAsync())
				.Where(umt => umt.UserId == userId && umt.CreationDate.ToString("yyyy-MM-dd") == formattedDate)
				.ToList();

			if (userMt.Count >= 3)
			{
				return new UserMedicalTestCreateVM { UserId = userId, CreatedToday = true };
			}
			return new UserMedicalTestCreateVM { UserId = userId, CreatedToday = false };
		}

		// GETS USER BODY MEASUREMENTS EDIT VIEW MODEL
		public async Task<UserMedicalTestCreateVM> GeUserMedicalTestEditVM(int medicalTestId)
		{
			return mapper.Map<UserMedicalTestCreateVM>(await GetAsync(medicalTestId));
		}

		// GETS USER BODY MEASUREMENTS DELETE VIEW MODEL
		public async Task<UserMedicalTestDeleteVM> GetUserMedicalTestDeleteVM(int medicalTestId)
		{
			return mapper.Map<UserMedicalTestDeleteVM>(await GetAsync(medicalTestId));
		}

		// CREATES NEW USER BODY MEASUREMENTS ENTITY
		public async Task CreateUserMedicalTestAsync(UserMedicalTestCreateVM userMedicalTestCreateVM, IFormFile? file)
		{
			if (file != null)
			{
				var fileUrl = await googleDriveService.UploadMedicalTestFileAsync(file);
				userMedicalTestCreateVM.FileUrl = fileUrl;
			}
			await AddAsync(mapper.Map<UserMedicalTest>(userMedicalTestCreateVM));
		}

		// EDITS EXSITING USER BODY MEASUREMENTS ENTITY
		public async Task EditUserMedicalTestAsync(UserMedicalTestCreateVM userMedicalTestCreateVM, IFormFile? file)
		{
			if (file != null)
			{
				var fileUrl = await googleDriveService.UploadMedicalTestFileAsync(file);
				userMedicalTestCreateVM.FileUrl = fileUrl;
			}
			await UpdateAsync(mapper.Map<UserMedicalTest>(userMedicalTestCreateVM));
		}

		// DELETES USER BODY MEASUREMENTS ENTITY
		public async Task DeleteUserMedicalTestAsync(UserMedicalTestDeleteVM userMedicalTestDeleteVM)
		{
			if (userMedicalTestDeleteVM.FileUrl != null)
			{
				await googleDriveService.RemoveFileAsync(userMedicalTestDeleteVM.FileUrl);
			}
			await DeleteAsync(userMedicalTestDeleteVM.Id);
		}
	}
}
