﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using EliteAthleteAppShared.Contracts;
using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.TrainingExercise;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.AspNetCore.Http;

namespace EliteAthleteAppShared.Repositories
{
	public class TrainingExerciseRepository : GenericRepository<TrainingExercise>, ITrainingExerciseRepository
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;
		private readonly UserManager<User> userManager;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly ITrainingExerciseCategoryRepository exerciseCategoryRepository;
		private readonly ITrainingExerciseMuscleGroupRepository exerciseMuscleGroupRepository;
		private readonly ITrainingExerciseMediaRepository exerciseMediaRepository;
		private readonly IYoutubeService youtubeService;

		public TrainingExerciseRepository(ApplicationDbContext context,
			IMapper mapper,
			UserManager<User> userManager,
			IHttpContextAccessor httpContextAccessor,
			ITrainingExerciseCategoryRepository exerciseCategoryRepository,
			ITrainingExerciseMuscleGroupRepository exerciseMuscleGroupRepository,
			ITrainingExerciseMediaRepository exerciseMediaRepository,
			IYoutubeService youtubeService) : base(context)
		{
			this.context = context;
			this.mapper = mapper;
			this.userManager = userManager;
			this.httpContextAccessor = httpContextAccessor;
			this.exerciseCategoryRepository = exerciseCategoryRepository;
			this.exerciseMuscleGroupRepository = exerciseMuscleGroupRepository;
			this.exerciseMediaRepository = exerciseMediaRepository;
			this.youtubeService = youtubeService;
		}

		// GETS EXERCISE INDEX VIEW MODEL (COACH ID)
		public async Task<TrainingExerciseIndexVM> GetExerciseIndexVMAsync(int? exerciseMediaId)
		{
			var coachId = (await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User)).Id;
			var privateExerciseCount = (await GetExerciseVMsAsync(coachId)).Count();

			return new TrainingExerciseIndexVM { CoachId = coachId, ExerciseMediaId = exerciseMediaId, PrivateExerciseCount = privateExerciseCount };
		}

		// GETS LIST OF PUBLIC OR PRIVATE EXERCISES
		public async Task<List<TrainingExerciseVM>> GetExerciseVMsAsync(string? coachId)
		{
			var exercises = (await GetAllAsync()).Where(e => e.CoachId == coachId).ToList();
			var exerciseVMs = mapper.Map<List<TrainingExerciseVM>>(exercises);

			for (int i = 0; i < exerciseVMs.Count; i++)
			{
				exerciseVMs[i] = await GetExerciseForeignEntitiesAsync(exerciseVMs[i], exercises[i]);
			}

			return exerciseVMs;
		}

		// GETS LIST OF PRIVATE EXERCISES SET TO PUBLIC
		public async Task<List<TrainingExerciseVM>> GetExerciseAdminVMsAsync()
		{
			var exercises = (await GetAllAsync()).Where(e => e.CoachId != null && e.SetAsPublic == true).ToList();
			var exerciseVMs = mapper.Map<List<TrainingExerciseVM>>(exercises);

			for (int i = 0; i < exerciseVMs.Count; i++)
			{
				exerciseVMs[i] = await GetExerciseForeignEntitiesAsync(exerciseVMs[i], exercises[i]);
			}

			return exerciseVMs;
		}

		// GETS SET EXERCISE AS PUBLIC VIEW MODEL
		public async Task<TrainingExerciseAsPublicVM> GetExerciseAsPublicVMAsync(int? trainingExerciseId)
		{
			return new TrainingExerciseAsPublicVM { TrainingExerciseVM = mapper.Map<TrainingExerciseVM>(await GetAsync(trainingExerciseId)) };
		}

		// GETS EXERCISE CREATE VIEW MODEL
		public async Task<TrainingExerciseCreateVM> GetExerciseCreateVMAsync(int? privateExerciseCount)
		{
			var trainingExerciseCreateVM = await GetExerciseCreateSelectListsAsync(null);
			var coach = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
			var subscription = await context.Set<UserSubscription>().FindAsync(coach.UserSubscriptionId);

			if (privateExerciseCount >= subscription.PrivateExerciseLimit)
			{
				trainingExerciseCreateVM.ReachedExerciseLimit = true;
			}

			return trainingExerciseCreateVM;
		}

		// GETS EXERCISE EDIT VIEW MODEL
		public async Task<TrainingExerciseCreateVM> GetExerciseEditVMAsync(int id)
		{
			return await GetExerciseCreateSelectListsAsync(mapper.Map<TrainingExerciseCreateVM>(await GetAsync(id)));
		}

		// GETS EXERCISE DETAILS VIEW MODEL
		public async Task<TrainingExerciseVM?> GetExerciseDetailsVMAsync(int id)
		{
			var exercise = await GetAsync(id);
			if (exercise != null)
			{
				var exerciseVM = mapper.Map<TrainingExerciseVM>(exercise);
				exerciseVM = await GetExerciseForeignEntitiesAsync(exerciseVM, exercise);
				exerciseVM.YoutubeLink = youtubeService.GetEmbeddedYouTubeLink(exerciseVM.YoutubeLink);
				return exerciseVM;
			}
			return null;
		}

		// GETS EXERCISE DELETE VIEW MODEL
		public async Task<TrainingExerciseDeleteVM> GetExerciseDeleteVMAsync(int id)
		{
			return mapper.Map<TrainingExerciseDeleteVM>(await GetAsync(id));
		}

		// CREATES A NEW DATABASE ENTITY IN THE EXERCISE TABLE
		public async Task CreateExerciseAsync(TrainingExerciseCreateVM exerciseCreateVM)
		{
			List<string?> imageUrls = [null, null, null];
			var trainingExerciseMedia = new TrainingExerciseMedia { ImageUrls = imageUrls };
			await exerciseMediaRepository.AddAsync(trainingExerciseMedia);
			exerciseCreateVM.ExerciseMediaId = trainingExerciseMedia.Id;
			await AddAsync(mapper.Map<TrainingExercise>(exerciseCreateVM));
		}

		// EDITS EXISTING DATABASE ENTITY IN THE EXERCISE TABLE
		public async Task EditExerciseAsync(TrainingExerciseCreateVM exerciseCreateVM)
		{
			await UpdateAsync(mapper.Map<TrainingExercise>(exerciseCreateVM));
		}

		// DELETES EXSITING EXERCUSE FROM THE DATABASE
		public async Task DeleteExerciseAsync(TrainingExerciseDeleteVM trainingExerciseDeleteVM)
		{
			await exerciseMediaRepository.DeleteExerciseMediaAsync(trainingExerciseDeleteVM.ExerciseMediaId);
			await DeleteAsync(trainingExerciseDeleteVM.Id);
		}

		public async Task SetExerciseAsPublic(int trainingExerciseId)
		{
			var exercise = await GetAsync(trainingExerciseId);
			exercise.CoachId = null;
			exercise.SetAsPublic = true;
			await UpdateAsync(exercise);
		}

		// PRIVATE METHODS BELOW

		// GETS THE CATEGORY AND MUSCLE GROUP ENTITIES VOR EXERCISE VIEW MODEL
		private async Task<TrainingExerciseVM> GetExerciseForeignEntitiesAsync(TrainingExerciseVM exerciseVM, TrainingExercise exercise)
		{
			exerciseVM.ExerciseCategory = mapper.Map<TrainingExerciseCategoryVM>(await exerciseCategoryRepository.GetAsync(exercise.ExerciseCategoryId));
			exerciseVM.ExerciseMuscleGroup = mapper.Map<TrainingExerciseMuscleGroupVM>(await exerciseMuscleGroupRepository.GetAsync(exercise.ExerciseMuscleGroupId));
			exerciseVM.ExerciseMediaVM = mapper.Map<TrainingExerciseMediaVM>(await exerciseMediaRepository.GetAsync(exercise.ExerciseMediaId));

			return exerciseVM;
		}

		// GETS EXERCISE SELECT LIST FOR CREATE AND EDIT VIEW MODELS
		private async Task<TrainingExerciseCreateVM> GetExerciseCreateSelectListsAsync(TrainingExerciseCreateVM? exerciseCreateVM)
		{
			if (exerciseCreateVM == null)
			{
				exerciseCreateVM = new TrainingExerciseCreateVM();
			}

			exerciseCreateVM.CoachId = (await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User)).Id;
			exerciseCreateVM.AvailableCategories = new SelectList(mapper.Map<List<TrainingExerciseCategoryVM>>(await exerciseCategoryRepository
				.GetAllAsync())
				.OrderBy(e => e.Name), "Id", "Name");
			exerciseCreateVM.AvailableMuscleGroups = new SelectList(mapper.Map<List<TrainingExerciseMuscleGroupVM>>(await exerciseMuscleGroupRepository
				.GetAllAsync())
				.OrderBy(e => e.Name), "Id", "Name");

			return exerciseCreateVM;
		}

	}
}
