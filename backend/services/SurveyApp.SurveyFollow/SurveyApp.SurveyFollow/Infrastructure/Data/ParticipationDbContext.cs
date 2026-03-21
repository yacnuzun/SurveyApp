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
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
