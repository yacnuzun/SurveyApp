using Autofac;
using Microsoft.EntityFrameworkCore;
using SurveyApp.ReportingService.Application.Services;
using SurveyApp.ReportingService.Infrastructure.Data;
using SurveyApp.ReportingService.Infrastructure.Repositories;
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
            builder.RegisterType<ReportManager>().As<IReportService>();

            #endregion

            builder.RegisterType<EfUnitOfWork<ReportingDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();

            #region repos
            builder.RegisterType<ReportRepository>().As<IReportRepository>();
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
