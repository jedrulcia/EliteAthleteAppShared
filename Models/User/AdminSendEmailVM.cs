﻿using System.ComponentModel.DataAnnotations;

namespace EliteAthleteAppShared.Models.User
{
    public class AdminSendEmailVM
    {
        // IDs
        public string? AdminId { get; set; }
        public string? UserId { get; set; }

        // STRINGS

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
