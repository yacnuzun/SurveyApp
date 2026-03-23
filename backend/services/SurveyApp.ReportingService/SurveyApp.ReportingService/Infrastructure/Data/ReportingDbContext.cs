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
            base.OnModelCreating(modelBuilder);
        }
    }
}
