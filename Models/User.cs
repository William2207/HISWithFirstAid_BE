using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace FirstAidAPI.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Points { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? Gender { get; set; }  // "Male", "Female", "Other"
        public string? Address { get; set; }
        public string? IdCard { get; set; }  // Số CCCD

        // Navigation properties
        public List<UserScenarioProgress> ScenarioProgresses { get; set; } = new List<UserScenarioProgress>();

        public List<UserTechniqueProgress> TechniqueProgresses { get; set; } = new List<UserTechniqueProgress>();
        public List<SavedTechnique> SavedTechniques { get; set; } = new List<SavedTechnique>();
        public List<ScenarioAttempt> ScenarioAttempts { get; set; } = new List<ScenarioAttempt>();

        // Navigate to roles
        public Patient? Patient { get; set; }

        public Doctor? Doctor { get; set; }
        public Nurse? Nurse { get; set; }
        public Receptionist? Receptionist { get; set; }
    }
}
