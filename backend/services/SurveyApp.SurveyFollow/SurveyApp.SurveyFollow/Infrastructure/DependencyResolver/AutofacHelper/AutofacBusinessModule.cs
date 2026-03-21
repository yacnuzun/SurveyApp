using Autofac;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyFollow.Infrastructure.Data;
using System.Reflection;
using Module = Autofac.Module;

namespace SurveyApp.SurveyFollow.Infrastructure.DependencyResolver.AutofacHelper
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var optionsBuilder = new DbContextOptionsBuilder<ParticipationDbContext>();
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                return new ParticipationDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();
        }
    }
}
