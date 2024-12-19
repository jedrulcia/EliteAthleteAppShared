using AutoMapper;
using EliteAthleteAppShared.Data;
using EliteAthleteAppShared.Models.TrainingExercise;
using EliteAthleteAppShared.Models.TrainingModule;
using EliteAthleteAppShared.Models.TrainingOrm;
using EliteAthleteAppShared.Models.TrainingPlan;
using EliteAthleteAppShared.Models.User;
using EliteAthleteAppShared.Models.UserBodyAnalysis;
using EliteAthleteAppShared.Models.UserBodyMeasurements;
using EliteAthleteAppShared.Models.UserChat;
using EliteAthleteAppShared.Models.UserMedicalTest;
using Microsoft.AspNetCore.Identity;

namespace EliteAthleteAppShared.Configurations
{
    public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			// USER MODULE MAPPING
			CreateMap<User, UserVM>().ReverseMap();
			CreateMap<User, UserInfoVM>().ReverseMap();

			CreateMap<UserChat, UserChatVM>().ReverseMap();

			CreateMap<UserBodyAnalysis, UserBodyAnalysisVM>().ReverseMap();
			CreateMap<UserBodyAnalysis, UserBodyAnalysisCreateVM>().ReverseMap();
			CreateMap<UserBodyAnalysis, UserBodyAnalysisDeleteVM>().ReverseMap();

			CreateMap<UserMedicalTest, UserMedicalTestVM>().ReverseMap();
			CreateMap<UserMedicalTest, UserMedicalTestCreateVM>().ReverseMap();
			CreateMap<UserMedicalTest, UserMedicalTestDeleteVM>().ReverseMap();

			CreateMap<UserBodyMeasurements, UserBodyMeasurementsVM>().ReverseMap();
			CreateMap<UserBodyMeasurements, UserBodyMeasurementsCreateVM>().ReverseMap();
			CreateMap<UserBodyMeasurements, UserBodyMeasurementsDeleteVM>().ReverseMap();

			// TRAINING EXERCISE MAPPING
			CreateMap<TrainingExercise, TrainingExerciseVM>().ReverseMap();
			CreateMap<TrainingExercise, TrainingExerciseIndexVM>().ReverseMap();
			CreateMap<TrainingExercise, TrainingExerciseCreateVM>().ReverseMap();
			CreateMap<TrainingExercise, TrainingExerciseDeleteVM>().ReverseMap();

			CreateMap<TrainingExerciseMedia, TrainingExerciseMediaVM>().ReverseMap();
			CreateMap<TrainingExerciseMedia, TrainingExerciseMediaCreateVM>().ReverseMap();

			CreateMap<TrainingExerciseCategory, TrainingExerciseCategoryVM>().ReverseMap();
			CreateMap<TrainingExerciseMuscleGroup, TrainingExerciseMuscleGroupVM>().ReverseMap();

			// TRAINING PLAN MAPPING
			CreateMap<TrainingPlan, TrainingPlanVM>().ReverseMap();
            CreateMap<TrainingPlan, TrainingPlanIndexVM>().ReverseMap();
			CreateMap<TrainingPlan, TrainingPlanCreateVM>().ReverseMap();
			CreateMap<TrainingPlan, TrainingPlanDetailsVM>().ReverseMap();
			CreateMap<TrainingPlan, TrainingPlanChangeStatusVM>().ReverseMap();

			CreateMap<TrainingPlanPhase, TrainingPlanPhaseVM>().ReverseMap();
			CreateMap<TrainingPlanExerciseDetail, TrainingPlanExerciseDetailVM>().ReverseMap();
			CreateMap<TrainingPlanExerciseDetail, TrainingPlanAddExerciseVM>().ReverseMap();

			// TRAINING MODULE MAPPING
			CreateMap<TrainingModule, TrainingModuleVM>().ReverseMap();
			CreateMap<TrainingModule, TrainingModuleCreateVM>().ReverseMap();
			CreateMap<TrainingModule, TrainingModuleDeleteVM>().ReverseMap();


			// TRAINING ORM MAPPING
			CreateMap<TrainingOrm, TrainingOrmVM>().ReverseMap();
			CreateMap<TrainingOrm, TrainingOrmCreateVM>().ReverseMap();
			CreateMap<TrainingOrm, TrainingOrmDeleteVM>().ReverseMap();
		}
	}
}
