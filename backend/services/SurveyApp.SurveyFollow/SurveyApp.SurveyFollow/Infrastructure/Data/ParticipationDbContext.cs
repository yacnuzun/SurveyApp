using Microsoft.EntityFrameworkCore;
using SurveyApp.SurveyFollow.Domain.Entities;

namespace SurveyApp.SurveyFollow.Infrastructure.Data
{
    public class ParticipationDbContext : DbContext
    {
        public ParticipationDbContext(DbContextOptions<ParticipationDbContext> options) : base(options) { }

        public DbSet<Participation> Participations { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Participation>(e =>
            {
                e.HasKey(x => x.Id);
                // Unique index — DB seviyesinde tekrar doldurma engeli
                e.HasIndex(x => new { x.SurveyId, x.UserId }).IsUnique();
                e.HasMany(x => x.Answers)
                 .WithOne(x => x.Participation)
                 .HasForeignKey(x => x.ParticipationId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Answer>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.TextAnswer).HasMaxLength(2000);
                // QuestionId ve OptionId — int FK, EF FK constraint YOK
                // Neden: Bu DB'de Questions ve TemplateOptions tablosu yok
                // HasForeignKey yazılmıyor, sadece property olarak kalıyor
            });
        }

    }
}
