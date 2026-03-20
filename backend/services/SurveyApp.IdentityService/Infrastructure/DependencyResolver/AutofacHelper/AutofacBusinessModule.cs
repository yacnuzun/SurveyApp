using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyApp.IdentityService.Infrastructure.Data;
using SurveyApp.IdentityService.Infrastructure.Helpers.JWT;
using SurveyApp.IdentityService.Infrastructure.Repositories.Implemantations;
using SurveyApp.IdentityService.Infrastructure.Repositories.Interfaces;
using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.Shared.Persistance.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Module = Autofac.Module;

namespace SurveyApp.IdentityService.Infrastructure.DependencyResolver.AutofacHelper
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
            //builder.RegisterType<AuthManager>().As<IAuthService>();

            builder.RegisterType<EfUnitOfWork<IdentityDbContext>>().As<IUnitOfWork>();

            // Repositories
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<OperationClaimRepository>().As<IOperationClaimRepository>();
            builder.RegisterType<UserOperationClaimRepository>().As<IUserOperationClaimRepository>();

            // Managers (Services)
            //builder.RegisterType<UserManager>().As<IUserService>();

            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                return new IdentityDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();
        }
    }
}
