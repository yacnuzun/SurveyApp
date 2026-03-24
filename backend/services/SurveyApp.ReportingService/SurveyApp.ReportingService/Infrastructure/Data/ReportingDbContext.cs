using Microsoft.EntityFrameworkCore;
using SurveyApp.ReportingService.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SurveyApp.ReportingService.Infrastructure.Data
{
    public class ReportingDbContext : DbContext
    {
        public ReportingDbContext(DbContextOptions<ReportingDbContext> options) : base(options) { }

        public DbSet<SurveyStatistic> SurveyStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SurveyStatistic>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.QuestionText).IsRequired().HasMaxLength(500);
                e.Property(x => x.OptionText).HasMaxLength(200);
                // SurveyId sadece int — join yok, denormalized veri
                e.HasIndex(x => x.SurveyId);
            });
        }
    }
}
