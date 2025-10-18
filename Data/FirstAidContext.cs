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
        public DbSet<TechniqueStep> TechniqueSteps { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Technique>()
                .HasMany(t => t.TechniqueSteps)
                .WithOne(ts => ts.Technique)
                .HasForeignKey(s => s.TechniqueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Scenario>()
                .HasMany(s => s.Techniques)
                .WithOne(t => t.Scenario)
                .HasForeignKey(s => s.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizQuestion>()
                .HasMany(q => q.AnswerOptions)
                .WithOne()
                .HasForeignKey(a => a.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
