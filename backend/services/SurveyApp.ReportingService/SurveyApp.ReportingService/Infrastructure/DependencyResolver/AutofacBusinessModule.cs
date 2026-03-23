using Autofac;
using Microsoft.EntityFrameworkCore;
using SurveyApp.ReportingService.Infrastructure.Data;
using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.Shared.Persistance.Interfaces;
using System.Reflection;
using Module = Autofac.Module;

namespace SurveyApp.ReportingService.Infrastructure.DependencyResolver
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region services
            
            #endregion

            builder.RegisterType<EfUnitOfWork<ReportingDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();

            #region repos
            //builder.RegisterType<UserRepository>().As<IUserRepository>();
            //builder.RegisterType<OperationClaimRepository>().As<IOperationClaimRepository>();
            //builder.RegisterType<UserOperationClaimRepository>().As<IUserOperationClaimRepository>();
            #endregion

            #region validators
            //builder.RegisterType<ClaimValidator>().As<IValidator<ClaimDto>>().InstancePerLifetimeScope();
            //builder.RegisterType<RegisterUserValidator>().As<IValidator<UserForRegisterDto>>().InstancePerLifetimeScope();
            #endregion

            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var optionsBuilder = new DbContextOptionsBuilder<ReportingDbContext>();
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                return new ReportingDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();
        }
    }
}
