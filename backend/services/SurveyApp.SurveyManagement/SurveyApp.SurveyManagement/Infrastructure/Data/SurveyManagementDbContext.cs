using Microsoft.EntityFrameworkCore;
using SurveyApp.SurveyManagement.Domain.Entities;

namespace SurveyApp.SurveyManagement.Infrastructure.Data
{
    public class SurveyManagementDbContext : DbContext
    {
        public SurveyManagementDbContext(DbContextOptions<SurveyManagementDbContext> options) : base(options) { }

        public DbSet<Option> Options { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Survey> Surveys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<Question>()
            .HasOne(q => q.Survey)
            .WithMany(s => s.Questions)
            .HasForeignKey(q => q.SurveyId);

            modelBuilder.Entity<Option>()
                .HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
