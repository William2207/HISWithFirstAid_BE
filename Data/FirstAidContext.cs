using Microsoft.EntityFrameworkCore;
using FirstAidAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FirstAidAPI.Data

{
    public class FirstAidContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public FirstAidContext(DbContextOptions<FirstAidContext> options)
            : base(options)
        {
        }

        public DbSet<Technique> Techniques { get; set; }
        public DbSet<TechniqueStep> TechniqueSteps { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<ScenarioStep> ScenarioSteps { get; set; }
        public DbSet<StepOption> StepOptions { get; set; }
        public DbSet<UserScenarioProgress> UserScenarioProgresses { get; set; }
        public DbSet<UserTechniqueProgress> UserTechniqueProgresses { get; set; }
        public DbSet<SavedTechnique> SavedTechniques { get; set; }
        public DbSet<ScenarioAttempt> ScenarioAttempts { get; set; }
        public DbSet<StepAnswer> StepAnswers { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }

        public DbSet<TechniqueType> TechniqueTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Đổi tên các bảng của Identity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users"); // Đổi AspNetUsers -> Users
            });

            modelBuilder.Entity<IdentityRole<int>>(entity =>
            {
                entity.ToTable(name: "Roles"); // Đổi AspNetRoles -> Roles
            });

            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles"); // Đổi AspNetUserRoles -> UserRoles
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims"); // Đổi AspNetUserClaims -> UserClaims
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins"); // Đổi AspNetUserLogins -> UserLogins
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims"); // Đổi AspNetRoleClaims -> RoleClaims
            });

            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens"); // Đổi AspNetUserTokens -> UserTokens
            });

            modelBuilder.Entity<QuizQuestion>()
                .HasOne(q => q.Technique)
                .WithMany(t => t.QuizQuestions)
                .HasForeignKey(q => q.TechniqueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Technique>()
                .HasMany(t => t.TechniqueSteps)
                .WithOne(ts => ts.Technique)
                .HasForeignKey(s => s.TechniqueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Technique>()
                .HasOne(t => t.Type)
                .WithMany(tt => tt.Techniques)
                .HasForeignKey(t => t.TechniqueTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuizQuestion>()
                .HasMany(q => q.AnswerOptions)
                .WithOne(a => a.QuizQuestion)
                .HasForeignKey(a => a.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Scenario>()
                .HasMany(s => s.ScenarioSteps)
                .WithOne(ss => ss.Scenario)
                .HasForeignKey(ss => ss.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScenarioStep>()
                .HasMany(ss => ss.Options)
                .WithOne(so => so.Step)
                .HasForeignKey(so => so.StepId)
                .OnDelete(DeleteBehavior.Cascade);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(u => u.Role)
                    .HasMaxLength(50)
                    .HasDefaultValue("User");

                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(u => u.IsActive)
                    .HasDefaultValue(true);

                entity.Property(u => u.Avatar)
                    .HasMaxLength(500);

                entity.Property(u => u.DateOfBirth);
            });

            // UserScenarioProgress configuration
            modelBuilder.Entity<UserScenarioProgress>(entity =>
            {
                entity.HasKey(usp => usp.Id);

                // Tạo composite index để tránh duplicate progress records
                entity.HasIndex(usp => new { usp.UserId, usp.ScenarioId })
                    .IsUnique();

                entity.HasOne(usp => usp.User)
                    .WithMany(u => u.ScenarioProgresses)
                    .HasForeignKey(usp => usp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(usp => usp.Scenario)
                    .WithMany()
                    .HasForeignKey(usp => usp.ScenarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(usp => usp.Status)
                    .HasDefaultValue(false);

                entity.Property(usp => usp.HighestScore)
                    .HasDefaultValue(0);

                entity.Property(usp => usp.LastAccessedAt)
                    .HasDefaultValueSql("NOW()");
            });

            // UserTechniqueProgress configuration
            modelBuilder.Entity<UserTechniqueProgress>(entity =>
            {
                entity.HasKey(utp => utp.Id);

                // Tạo composite index để tránh duplicate progress records
                entity.HasIndex(utp => new { utp.UserId, utp.TechniqueId })
                    .IsUnique();

                entity.HasOne(utp => utp.User)
                    .WithMany(u => u.TechniqueProgresses)
                    .HasForeignKey(utp => utp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(utp => utp.Technique)
                    .WithMany()
                    .HasForeignKey(utp => utp.TechniqueId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(utp => utp.Status)
                    .HasDefaultValue(false);

                entity.Property(utp => utp.IsVideoWatched)
                    .HasDefaultValue(false);

                entity.Property(utp => utp.LastAccessedAt)
                    .HasDefaultValueSql("NOW()");
            });

            // SavedTechnique configuration
            modelBuilder.Entity<SavedTechnique>(entity =>
            {
                entity.HasKey(st => st.Id);

                // Tránh user lưu duplicate technique
                entity.HasIndex(st => new { st.UserId, st.TechniqueId })
                    .IsUnique();

                entity.HasOne(st => st.User)
                    .WithMany(u => u.SavedTechniques)
                    .HasForeignKey(st => st.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(st => st.Technique)
                    .WithMany()
                    .HasForeignKey(st => st.TechniqueId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(st => st.SavedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(st => st.Priority)
                    .HasDefaultValue(0);
            });

            // ScenarioAttempt configuration
            modelBuilder.Entity<ScenarioAttempt>(entity =>
            {
                entity.HasKey(sa => sa.Id);

                // Index để query nhanh attempts của user
                entity.HasIndex(sa => new { sa.UserId, sa.AttemptedAt });

                entity.HasOne(sa => sa.User)
                    .WithMany(u => u.ScenarioAttempts)
                    .HasForeignKey(sa => sa.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sa => sa.Scenario)
                    .WithMany()
                    .HasForeignKey(sa => sa.ScenarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(sa => sa.AttemptedAt)
                    .HasDefaultValueSql("NOW()");
            });

            // StepAnswer configuration
            modelBuilder.Entity<StepAnswer>(entity =>
            {
                entity.HasKey(sa => sa.Id);

                entity.HasOne(sa => sa.ScenarioAttempt)
                    .WithMany(attempt => attempt.StepAnswers)
                    .HasForeignKey(sa => sa.ScenarioAttemptId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sa => sa.Step)
                    .WithMany()
                    .HasForeignKey(sa => sa.StepId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(sa => sa.AnsweredAt)
                    .HasDefaultValueSql("NOW()");
            });

            // UserAchievement configuration
            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.HasKey(ua => ua.Id);

                // Tránh user nhận duplicate achievement
                entity.HasIndex(ua => new { ua.UserId, ua.AchievementType })
                    .IsUnique();

                entity.HasOne(ua => ua.User)
                    .WithMany()
                    .HasForeignKey(ua => ua.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(ua => ua.AchievementType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(ua => ua.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(ua => ua.EarnedAt)
                    .HasDefaultValueSql("NOW()");
            });

            ScenarioSeeder.SeedScenarios(modelBuilder);
        }
    }
}