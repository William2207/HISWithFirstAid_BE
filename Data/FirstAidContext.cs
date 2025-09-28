using Microsoft.EntityFrameworkCore;
using FirstAidAPI.Models;

namespace FirstAidAPI.Data

{
    public class FirstAidContext : DbContext
    {
        public FirstAidContext(DbContextOptions<FirstAidContext> options)
            : base(options)
        {
        }

        public DbSet<Technique> Techniques { get; set; }
        public DbSet<ScenarioStep> ScenarioSteps { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Technique>()
                .HasMany(t => t.ScenarioSteps)
                .WithOne()
                .HasForeignKey(s => s.TechniqueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Scenario>()
                .HasMany(s => s.ScenarioSteps)
                .WithOne()
                .HasForeignKey(s => s.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizQuestion>()
                .HasMany(q => q.AnswerOptions)
                .WithOne()
                .HasForeignKey(a => a.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScenarioStep>()
                .Property(s => s.TechniqueId)
                .IsRequired(false);

            modelBuilder.Entity<ScenarioStep>()
                .Property(s => s.ScenarioId)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
