using Microsoft.EntityFrameworkCore;
using SurveyApp.ReportingService.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SurveyApp.ReportingService.Infrastructure.Data
{
    public class ReportingDbContext : DbContext
    {
        public ReportingDbContext(DbContextOptions<ReportingDbContext> options) : base(options) { }

        public DbSet<SurveyStatistic> Statistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<SurveyStatistic>().HasData(
                new SurveyStatistic { Id = 1, SurveyId = 1, QuestionText = "Memnuniyet", OptionText = "Evet", Count = 10 },
                new SurveyStatistic { Id = 2, SurveyId = 1, QuestionText = "Memnuniyet", OptionText = "Hayır", Count = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
