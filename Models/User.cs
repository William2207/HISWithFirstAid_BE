using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace FirstAidAPI.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }

        //public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public List<UserScenarioProgress> ScenarioProgresses { get; set; } = new List<UserScenarioProgress>();

        public List<UserTechniqueProgress> TechniqueProgresses { get; set; } = new List<UserTechniqueProgress>();
        public List<SavedTechnique> SavedTechniques { get; set; } = new List<SavedTechnique>();
        public List<ScenarioAttempt> ScenarioAttempts { get; set; } = new List<ScenarioAttempt>();
    }
}