using Microsoft.EntityFrameworkCore;
using SurveyApp.IdentityService.Domain.Entities;

namespace SurveyApp.IdentityService.Infrastructure.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        // EmailTemplate ve FailureLog'u eğer kullanmayacaksan şimdilik eklemedim.
        // İhtiyacın olursa buraya eklenebilir.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
