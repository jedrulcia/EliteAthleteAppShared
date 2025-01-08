﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using EliteAthleteAppShared.Contracts;
using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.TrainingExercise;
using EliteAthleteAppShared.Models.TrainingPlan;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.IdentityModel.Tokens;
using EliteAthleteAppShared.Models;
using EliteAthleteAppShared.Models.User;
using EliteAthleteAppShared.Models.UserMedicalTest;

namespace EliteAthleteAppShared.Repositories
{
    public class TrainingPlanRepository : GenericRepository<TrainingPlan>, ITrainingPlanRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ITrainingExerciseRepository exerciseRepository;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITrainingPlanExerciseDetailRepository trainingPlanExerciseDetailRepository;
        private readonly ITrainingPlanPhaseRepository trainingPlanPhaseRepository;
        private readonly IYoutubeService youtubeService;

        public TrainingPlanRepository(ApplicationDbContext context,
            IMapper mapper,
            ITrainingExerciseRepository exerciseRepository,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            ITrainingPlanExerciseDetailRepository trainingPlanExerciseDetailRepository,
            ITrainingPlanPhaseRepository trainingPlanPhaseRepository,
            IYoutubeService youtubeService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.exerciseRepository = exerciseRepository;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.trainingPlanExerciseDetailRepository = trainingPlanExerciseDetailRepository;
            this.trainingPlanPhaseRepository = trainingPlanPhaseRepository;
            this.youtubeService = youtubeService;
        }

        // GETS A LIST OF SPECIFIC USER TRAINING PLANS BASED ON PROVIDED TRAINING PLAN IDs.
        public async Task<TrainingPlanIndexVM> GetTrainingPlanIndexVMAsync(int trainingModuleId)
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
            var coach = await userManager.FindByIdAsync(user.CoachId);

            var trainingPlanIndexVM = new TrainingPlanIndexVM
            {
                UserId = user.Id,
                CoachId = coach.Id,
                TrainingModuleId = trainingModuleId
            };

            return trainingPlanIndexVM;
        }

        public async Task<TrainingPlanListVM> GetTrainingPlanListVMAsync(List<int> trainingPlanIds)
        {
            var (trainingPlanVMs, progress) = await GetTrainingPlanVMsAndProgressAsync(trainingPlanIds);
            return (new TrainingPlanListVM { Progress = progress, TrainingPlanVMs = trainingPlanVMs });
        }

        // GETS THE TRAINING PLAN DETAILS VIEW MODEL FOR THE SPECIFIED TRAINING PLAN.
        public async Task<TrainingPlanDetailsVM> GetTrainingPlanDetailsVMAsync(int trainingPlanId)
        {
            var trainingPlan = await GetAsync(trainingPlanId);
            var trainingPlanDetailsVM = mapper.Map<TrainingPlanDetailsVM>(trainingPlan);

            trainingPlanDetailsVM.TrainingPlanExerciseDetailVMs = await GetTrainingPlanExerciseDetailVMsAsync(trainingPlan.TrainingPlanExerciseDetailIds, trainingPlanId);

            return trainingPlanDetailsVM;
        }

        public async Task<TrainingPlanChangeStatusVM> GetTrainingPlanChangeStatusVMAsync(int id)
        {
            return mapper.Map<TrainingPlanChangeStatusVM>(await GetAsync(id));
        }

        public async Task<TrainingPlanCopyVM> GetTrainingPlanCopyVMAsync(int? copyFromId, List<int> trainingPlanIds, int trainingModuleId)
        {
            List<TrainingPlanVM> trainingPlanVMs = new List<TrainingPlanVM>();

            foreach (int id in trainingPlanIds)
            {
                var trainingPlan = await GetAsync(id);
                trainingPlanVMs.Add(mapper.Map<TrainingPlanVM>(trainingPlan));
            }

            var trainingPlanCopyVM = new TrainingPlanCopyVM
            {
                TrainingModuleId = trainingModuleId,
                CopyFromId = copyFromId,
                TrainingPlanVMs = trainingPlanVMs
            };
            return trainingPlanCopyVM;
        }

        public async Task<TrainingPlanAddExerciseVM> GetTrainingPlanAddExerciseVMAsync(int trainingPlanId, string coachId)
        {
            var trainingPlan = await GetAsync(trainingPlanId);
            var trainingPlanAddExerciseVM = await GetTrainingPlanAddExerciseSelectListsAsync(trainingPlanId, coachId, null);

            if (trainingPlan.TrainingPlanExerciseDetailIds.Count >= 30)
            {
                trainingPlanAddExerciseVM.ReachedExerciseLimit = true;
            }

            return trainingPlanAddExerciseVM;
        }

        public async Task<TrainingPlanAddExerciseVM> GetTrainingPlanEditExerciseVMAsync(int trainingPlanId, string coachId, int trainingPlanExerciseDetailId)
        {
            var trainingPlan = await GetAsync(trainingPlanId);
            var trainingPlanAddExerciseVM = mapper.Map<TrainingPlanAddExerciseVM>(await trainingPlanExerciseDetailRepository.GetAsync(trainingPlanExerciseDetailId));
            var trainingPlanAddExerciseSelectLists = await GetTrainingPlanAddExerciseSelectListsAsync(trainingPlanId, coachId, trainingPlanAddExerciseVM);

            return trainingPlanAddExerciseVM;
        }

        public async Task<TrainingPlanRemoveExerciseVM> GetTrainingPlanRemoveExerciseVM(int trainingPlanId, int trainingPlanExerciseDetailId, string name)
        {
            var trainingPlanRemoveExerciseVM = new TrainingPlanRemoveExerciseVM
            {
                Name = name,
                Id = trainingPlanExerciseDetailId,
                TrainingPlanId = trainingPlanId
            };
            return trainingPlanRemoveExerciseVM;
        }

        public async Task<TrainingPlanDetailsVM?> GetDailyTrainingPlanVMAsync(string userId)
        {
            var trainingPlans = (await GetAllAsync()).Where(tp => tp.UserId == userId).ToList();

            var todayDate = DateTime.Today.ToString("dd-MM-yyyy");
            var trainingPlan = trainingPlans.Where(tp => tp.Date?.ToString("dd-MM-yyyy") == todayDate).FirstOrDefault();

            if (trainingPlan == null || trainingPlan.IsEmpty == true)
            {
                trainingPlan = trainingPlans
                    .Where(tp => tp.Date < DateTime.Now && tp.IsEmpty == false)
                    .OrderByDescending(tp => tp.Date)
                    .FirstOrDefault();
            }

            if (trainingPlan != null && trainingPlan.IsEmpty == false)
            {
                var trainingPlanDetailsVM = mapper.Map<TrainingPlanDetailsVM>(trainingPlan);
                trainingPlanDetailsVM.TrainingPlanExerciseDetailVMs = await GetTrainingPlanExerciseDetailVMsAsync(trainingPlan.TrainingPlanExerciseDetailIds, trainingPlan.Id);
                return trainingPlanDetailsVM;
            }
            return null;
        }

        // CREATES A NEW DATABASE ENTITY IN THE TRAINING PLAN TABLE AND RETURNS THE NEW ID.
        public async Task<int> CreateTrainingPlanAsync(TrainingPlanCreateVM trainingPlanCreateVM)
        {
            var trainingPlan = mapper.Map<TrainingPlan>(trainingPlanCreateVM);

            trainingPlan.TrainingPlanExerciseDetailIds = new List<int?>();
            trainingPlan.IsCompleted = false;
            trainingPlan.IsEmpty = true;

            await AddAsync(trainingPlan);
            return trainingPlan.Id;
        }

        // DELETES TRAINING PLAN AND ITS EXERCISE DETAILS
        public async Task DeleteTrainingPlanAndDetailsAsync(int trainingPlanId)
        {
            var trainingPlan = await GetAsync(trainingPlanId);
            var holder = trainingPlan.TrainingPlanExerciseDetailIds.ToList();

            for (int i = 0; i < holder.Count; i++)
            {
                await trainingPlanExerciseDetailRepository.DeleteAsync(holder[i].Value);
            }

            await DeleteAsync(trainingPlanId);
        }

        // ADDS AN EXERCISE TO THE SPECIFIED TRAINING PLAN.
        public async Task<TrainingPlanDetailsVM> AddExerciseToTrainingPlanAsync(TrainingPlanAddExerciseVM trainingPlanAddExerciseVM)
        {
            var exercise = await exerciseRepository.GetAsync(trainingPlanAddExerciseVM.ExerciseId);
            if (exercise == null)
            {
                return await GetTrainingPlanDetailsVMAsync(trainingPlanAddExerciseVM.TrainingPlanId);
            }

            var trainingPlanExerciseDetail = mapper.Map<TrainingPlanExerciseDetail>(trainingPlanAddExerciseVM);
            await trainingPlanExerciseDetailRepository.AddAsync(trainingPlanExerciseDetail);

            var trainingPlan = await GetAsync(trainingPlanAddExerciseVM.TrainingPlanId);
            trainingPlan.IsEmpty = false;
            trainingPlan.TrainingPlanExerciseDetailIds.Add(trainingPlanExerciseDetail.Id);
            await UpdateAsync(trainingPlan);

            return await GetTrainingPlanDetailsVMAsync(trainingPlanAddExerciseVM.TrainingPlanId);
        }

        // EDITS AN EXERCISE IN THE SPECIFIED TRAINING PLAN.
        public async Task<TrainingPlanDetailsVM> EditExerciseInTrainingPlanAsync(TrainingPlanAddExerciseVM trainingPlanAddExerciseVM)
        {
            var exercise = await exerciseRepository.GetAsync(trainingPlanAddExerciseVM.ExerciseId);
            if (exercise == null)
            {
                return await GetTrainingPlanDetailsVMAsync(trainingPlanAddExerciseVM.TrainingPlanId);
            }
            int? id = trainingPlanAddExerciseVM.Id;
            var trainingPlanExerciseDetail = mapper.Map<TrainingPlanExerciseDetail>(trainingPlanAddExerciseVM);
            await trainingPlanExerciseDetailRepository.UpdateAsync(trainingPlanExerciseDetail);

            return await GetTrainingPlanDetailsVMAsync(trainingPlanAddExerciseVM.TrainingPlanId);
        }

        // REMOVES AN EXERCISE FROM THE SPECIFIED TRAINING PLAN BASED ON TRAINING PLAN ID AND EXERCISE INDEX.
        public async Task RemoveExerciseFromTrainingPlanAsync(TrainingPlanRemoveExerciseVM trainingPlanRemoveExerciseVM)
        {
            var trainingPlan = await GetAsync(trainingPlanRemoveExerciseVM.TrainingPlanId);
            var trainingPlanExerciseDetail = await trainingPlanExerciseDetailRepository.GetAsync(trainingPlanRemoveExerciseVM.Id);

            trainingPlan.TrainingPlanExerciseDetailIds.Remove(trainingPlanRemoveExerciseVM.Id);
            await trainingPlanExerciseDetailRepository.DeleteAsync(trainingPlanRemoveExerciseVM.Id);
            if (trainingPlan.TrainingPlanExerciseDetailIds.Count == 0)
            {
                trainingPlan.IsEmpty = true;
            }
            await UpdateAsync(trainingPlan);
        }

        // CHANGES THE STATUS OF THE TRAINING PLAN (ACTIVE/NOT ACTIVE).
        public async Task CompleteTrainingPlanAsync(int trainingPlanId, string raport)
        {
            var trainingPlan = await GetAsync(trainingPlanId);
            trainingPlan.IsCompleted = true;
            trainingPlan.Raport = raport;
            await UpdateAsync(trainingPlan);
        }

        // COPIES A TRAINING PLAN TO ANOTHER TRAINING PLAN WITHIN THE SAME MODULE.
        public async Task CopyTrainingPlanAsync(int copyFromId, int copyToId)
        {
            var copyFromTrainingPlan = await GetAsync(copyFromId);
            var copyToTrainingPlan = await GetAsync(copyToId);

            foreach (var id in copyToTrainingPlan.TrainingPlanExerciseDetailIds)
            {
                await trainingPlanExerciseDetailRepository.DeleteAsync(id.Value);
            }

            copyToTrainingPlan.TrainingPlanExerciseDetailIds = new List<int?>();
            copyToTrainingPlan.IsEmpty = copyFromTrainingPlan.IsEmpty;

            foreach (var id in copyFromTrainingPlan.TrainingPlanExerciseDetailIds)
            {
                var copyToExerciseDetail = await trainingPlanExerciseDetailRepository.GetAsync(id);
                copyToExerciseDetail.Id = null;
                await trainingPlanExerciseDetailRepository.AddAsync(copyToExerciseDetail);
                copyToTrainingPlan.TrainingPlanExerciseDetailIds.Add(copyToExerciseDetail.Id);
            }
            await UpdateAsync(copyToTrainingPlan);
        }

        // METHODS NOT AVAILABLE OUTSIDE OF THE CLASS BELOW

        // GETS A LIST OF TRAINING PLAN EXERCISE DETAIL VM
        private async Task<List<TrainingPlanExerciseDetailVM>> GetTrainingPlanExerciseDetailVMsAsync(List<int?> trainingPlanExerciseDetailIds, int? trainingPlanId)
        {
            var trainingPlanExerciseDetailVMs = new List<TrainingPlanExerciseDetailVM>();
            List<int?> removedExerciseIds = new List<int?>();

            foreach (var id in trainingPlanExerciseDetailIds)
            {
                var trainingPlanExerciseDetailVM = mapper.Map<TrainingPlanExerciseDetailVM>(await trainingPlanExerciseDetailRepository.GetAsync(id));
                trainingPlanExerciseDetailVM.ExerciseVM = mapper.Map<TrainingExerciseVM>(await exerciseRepository.GetExerciseDetailsVMAsync(trainingPlanExerciseDetailVM.ExerciseId.Value));
                trainingPlanExerciseDetailVM.ExerciseVM.YoutubeLink = youtubeService.GetEmbeddedYouTubeLink(trainingPlanExerciseDetailVM.ExerciseVM.YoutubeLink);
                trainingPlanExerciseDetailVM.TrainingPlanPhaseVM = mapper.Map<TrainingPlanPhaseVM>(await trainingPlanPhaseRepository.GetAsync(trainingPlanExerciseDetailVM.TrainingPlanPhaseId));
                if (trainingPlanExerciseDetailVM.ExerciseVM != null)
                {
                    trainingPlanExerciseDetailVMs.Add(trainingPlanExerciseDetailVM);
                }
                else
                {
                    removedExerciseIds.Add(id);
                }
            }

            trainingPlanExerciseDetailVMs = SortTrainingPlanExerciseDetailVMs(trainingPlanExerciseDetailVMs);

            if (!removedExerciseIds.IsNullOrEmpty())
            {
                foreach (var id in removedExerciseIds)
                {
                    await RemoveExerciseFromTrainingPlanAsync(new TrainingPlanRemoveExerciseVM { TrainingPlanId = trainingPlanId, Id = id.Value });
                }
            }

            return trainingPlanExerciseDetailVMs;
        }

        // GETS THE LIST ON TRAINING PLAN VIEW MODELS AND COUNTS THE PROGRESS OF THE TRAINING MODULE
        private async Task<(List<TrainingPlanVM> trainingPlanVMs, int progress)> GetTrainingPlanVMsAndProgressAsync(List<int> trainingPlanIds)
        {
            List<TrainingPlanVM> trainingPlanVMs = new List<TrainingPlanVM>();

            int completed = 0, notCompleted = 0;
            float progress = 0;

            foreach (int id in trainingPlanIds)
            {
                var trainingPlanVM = (mapper.Map<TrainingPlanVM>(await GetAsync(id)));
                trainingPlanVMs.Add(trainingPlanVM);
                if (trainingPlanVM.IsCompleted)
                {
                    completed++;
                }
                else if (!trainingPlanVM.IsCompleted && !trainingPlanVM.IsEmpty)
                {
                    notCompleted++;
                }
            }

            if ((completed + notCompleted) != 0)
            {
                progress = ((float)completed / (float)(completed + notCompleted)) * 100;
            }

            return (trainingPlanVMs, (int)progress);
        }

        // GETS THE SELECT LISTS FOR CREATE/EDIT VIEW MODEL
        private async Task<TrainingPlanAddExerciseVM> GetTrainingPlanAddExerciseSelectListsAsync(int trainingPlanId, string coachId, TrainingPlanAddExerciseVM? trainingPlanAddExerciseVM)
        {
            if (trainingPlanAddExerciseVM == null)
            {
                trainingPlanAddExerciseVM = new TrainingPlanAddExerciseVM();
            }

            var availableExerciseVMs = mapper.Map<List<TrainingExerciseVM>>(await exerciseRepository.GetAllAsync()).Where(e => e.CoachId == null || e.CoachId == coachId).OrderBy(e => e.Name);
            var availableTrainingPlanPhaseVMs = mapper.Map<List<TrainingPlanPhaseVM>>(await trainingPlanPhaseRepository.GetAllAsync()).OrderBy(e => e.Id);

            trainingPlanAddExerciseVM.AvailableExercises = new SelectList(availableExerciseVMs, "Id", "Name");
            trainingPlanAddExerciseVM.AvailableTrainingPlanPhases = new SelectList(availableTrainingPlanPhaseVMs, "Id", "Name");
            trainingPlanAddExerciseVM.TrainingPlanId = trainingPlanId;
            return trainingPlanAddExerciseVM;
        }

        // SORTS ALL LISTS BY INDICES LIST
        private List<TrainingPlanExerciseDetailVM> SortTrainingPlanExerciseDetailVMs(List<TrainingPlanExerciseDetailVM> trainingPlanExerciseDetailVMs)
        {
            var sortedList = trainingPlanExerciseDetailVMs
                .Select(x => new
                {
                    TrainingPlanExerciseDetail = x,
                    NumberPart = ExtractNumber(x.Index),
                    LetterPart = ExtractLetters(x.Index)
                })
                .OrderBy(x => x.NumberPart)
                .ThenBy(x => x.LetterPart)
                .Select(x => x.TrainingPlanExerciseDetail)
                .ToList();

            return sortedList;
        }

        // EXTRACTS NUMBER FROM INDICES
        private int ExtractNumber(string str)
        {
            var numberString = new string(str.TakeWhile(char.IsDigit).ToArray());
            return int.TryParse(numberString, out var number) ? number : 0;
        }

        // EXTRACTS LETTER FROM INDICES
        private string ExtractLetters(string str)
        {
            var letterString = new string(str.SkipWhile(char.IsDigit).ToArray());
            return letterString;
        }
    }
}
