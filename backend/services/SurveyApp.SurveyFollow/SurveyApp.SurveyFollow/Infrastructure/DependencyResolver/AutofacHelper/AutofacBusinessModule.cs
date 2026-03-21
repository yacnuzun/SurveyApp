using Autofac;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyFollow.Application.Dto_s;
using SurveyApp.SurveyFollow.Application.Services;
using SurveyApp.SurveyFollow.Application.Validators;
using SurveyApp.SurveyFollow.Infrastructure.Data;
using SurveyApp.SurveyFollow.Infrastructure.Repositories.Implemantations;
using SurveyApp.SurveyFollow.Infrastructure.Repositories.Interfaces;
using System.Reflection;
using Module = Autofac.Module;

namespace SurveyApp.SurveyFollow.Infrastructure.DependencyResolver.AutofacHelper
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           

            #region services
            builder.RegisterType<ParticipationManager>().As<IParticipationService>().InstancePerLifetimeScope();
            #endregion

            builder.RegisterType<EfUnitOfWork<ParticipationDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();

            #region repos
            builder.RegisterType<ParticipationRepository>().As<IParticipationRepository>();
            builder.RegisterType<AnswerRepository>().As<IAnswerRepository>();
            #endregion

            #region validators
            builder.RegisterType<ParticipationForCreateDtoValidator>().As<IValidator<ParticipationForCreateDto>>().InstancePerLifetimeScope();
            #endregion
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
