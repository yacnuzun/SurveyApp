using Autofac;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyManagement.Infrastructure.Data;
using System.Reflection;
using Module = Autofac.Module;

namespace SurveyApp.SurveyManagement.Infrastructure.DependencyResolver.AutofacHelper
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var optionsBuilder = new DbContextOptionsBuilder<SurveyManagementDbContext>();
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                return new SurveyManagementDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();
        }
    }
}
