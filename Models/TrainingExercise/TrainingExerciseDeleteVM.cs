﻿using System.ComponentModel.DataAnnotations;

namespace EliteAthleteAppShared.Models.TrainingExercise
{
	public class TrainingExerciseDeleteVM
	{
		// IDs
		public int Id { get; set; }
		public int ExerciseMediaId { get; set; }

		// STRINGS

		[Display(Name = "Exercise")]
		public string Name { get; set; }
	}
}
