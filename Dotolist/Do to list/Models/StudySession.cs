using System;
using System.ComponentModel.DataAnnotations;

namespace DoToList.Models
{
    public class StudySession
    {
        [Key]
        public int Id { get; set; } // Unique Identifier

        public string Type { get; set; } // Study or Break
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int Duration => (int)(EndTime - StartTime).TotalMinutes; // Auto-calculated
        public string UserId { get; set; } // Stores the ID of the user who owns the session
    }
}
