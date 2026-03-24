using Microsoft.EntityFrameworkCore;
using SurveyApp.SurveyManagement.Domain.Entities;

namespace SurveyApp.SurveyManagement.Infrastructure.Data
{
    public class SurveyManagementDbContext : DbContext
    {
        public SurveyManagementDbContext(DbContextOptions<SurveyManagementDbContext> options) : base(options) { }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerTemplate> AnswerTemplates { get; set; }
        public DbSet<TemplateOption> TemplateOptions { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyUser> SurveyUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Survey>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).IsRequired().HasMaxLength(200);
                e.Property(x => x.Description).HasMaxLength(1000);
                // Participations navigation olmadığı için HasMany yazılmıyor
            });

            modelBuilder.Entity<AnswerTemplate>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.HasMany(x => x.Options)
                 .WithOne(x => x.AnswerTemplate)
                 .HasForeignKey(x => x.AnswerTemplateId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.Questions)
                 .WithOne(x => x.AnswerTemplate)
                 .HasForeignKey(x => x.AnswerTemplateId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TemplateOption>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Text).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<Question>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Text).IsRequired().HasMaxLength(500);
            });

            modelBuilder.Entity<SurveyQuestion>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.SurveyId, x.QuestionId }).IsUnique();
                e.HasOne(x => x.Survey)
                 .WithMany(x => x.SurveyQuestions)
                 .HasForeignKey(x => x.SurveyId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Question)
                 .WithMany(x => x.SurveyQuestions)
                 .HasForeignKey(x => x.QuestionId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SurveyUser>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.SurveyId, x.UserId }).IsUnique();
                e.HasOne(x => x.Survey)
                 .WithMany(x => x.AssignedUsers)
                 .HasForeignKey(x => x.SurveyId)
                 .OnDelete(DeleteBehavior.Cascade);
                // User navigation yok — HasOne yazılmıyor
                // UserId sadece int olarak saklanıyor
            });
        }

    }
}
