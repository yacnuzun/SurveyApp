using Autofac;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Application.Services.Implementations;
using SurveyApp.IdentityService.Application.Services.Interfaces;
using SurveyApp.IdentityService.Application.Validators;
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
            builder.RegisterType<JwtHelper>().As<ITokenHelper>().InstancePerLifetimeScope();

            #region services
            builder.RegisterType<AuthManager>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<UserManager>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>().InstancePerLifetimeScope();
            builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>().InstancePerLifetimeScope();
            #endregion

            builder.RegisterType<EfUnitOfWork<IdentityDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();

            #region repos
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<OperationClaimRepository>().As<IOperationClaimRepository>();
            builder.RegisterType<UserOperationClaimRepository>().As<IUserOperationClaimRepository>();
            #endregion

            #region validators
            builder.RegisterType<ClaimValidator>().As<IValidator<ClaimDto>>().InstancePerLifetimeScope();
            builder.RegisterType<RegisterUserValidator>().As<IValidator<UserForRegisterDto>>().InstancePerLifetimeScope();
            #endregion

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
