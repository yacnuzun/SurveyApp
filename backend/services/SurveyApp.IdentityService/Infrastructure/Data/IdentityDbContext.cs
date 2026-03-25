using Microsoft.EntityFrameworkCore;
using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.Shared.Helpers.Security.Hashing;

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

        public async Task SeedAsync()
        {
            // 1. Claim'ler
            if (!OperationClaims.Any())
            {
                OperationClaims.AddRange(
                    new OperationClaim { Name = "Admin" },
                    new OperationClaim { Name = "User" }
                );
                await SaveChangesAsync();
            }

            // 2. Admin user
            if (!Users.Any(u => u.Email == "admin@surveyapp.com"))
            {
                HashingHelper.CreatePasswordHash("Admin123!", out byte[] passwordHash, out byte[] passwordSalt);

                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "admin@surveyapp.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true
                };

                Users.Add(adminUser);
                await SaveChangesAsync();

                var adminClaim = OperationClaims.First(c => c.Name == "Admin");
                UserOperationClaims.Add(new UserOperationClaim
                {
                    UserId = adminUser.Id,
                    OperationClaimId = adminClaim.Id
                });
                await SaveChangesAsync();
            }

            // 3. Normal user'lar
            var userClaim = OperationClaims.First(c => c.Name == "User");

            var normalUsers = new[]
            {
            ("user1@surveyapp.com", "User1", "User123!"),
            ("user2@surveyapp.com", "User2", "User123!"),
            ("user3@surveyapp.com", "User3", "User123!"),
        };

            foreach (var (email, userName, password) in normalUsers)
            {
                if (!Users.Any(u => u.Email == email))
                {
                    HashingHelper.CreatePasswordHash(password, out byte[] hash, out byte[] salt);

                    var user = new User
                    {
                        UserName = userName,
                        Email = email,
                        PasswordHash = hash,
                        PasswordSalt = salt,
                        Status = true
                    };

                    Users.Add(user);
                    await SaveChangesAsync();

                    UserOperationClaims.Add(new UserOperationClaim
                    {
                        UserId = user.Id,
                        OperationClaimId = userClaim.Id
                    });
                    await SaveChangesAsync();
                }
            }
        }

    }
}
