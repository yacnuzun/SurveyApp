using Autofac;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyManagement.Application.Dto_s;
using SurveyApp.SurveyManagement.Application.Services;
using SurveyApp.SurveyManagement.Application.Validators;
using SurveyApp.SurveyManagement.Infrastructure.Data;
using SurveyApp.SurveyManagement.Infrastructure.Repositories.Implemantations;
using SurveyApp.SurveyManagement.Infrastructure.Repositories.Interfaces;
using System.Reflection;
using Module = Autofac.Module;

namespace SurveyApp.SurveyManagement.Infrastructure.DependencyResolver.AutofacHelper
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            #region services
            builder.RegisterType<SurveyManager>().As<ISurveyService>().InstancePerLifetimeScope();
            #endregion

            builder.RegisterType<EfUnitOfWork<SurveyManagementDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();

            #region repos
            builder.RegisterType<SurveyRepository>().As<ISurveyRepository>();
            builder.RegisterType<OptionRepository>().As<IOptionRepository>();
            builder.RegisterType<QuestionRepository>().As<IQuestionRepository>();
            #endregion

            #region validators
            builder.RegisterType<SurveyForCreateDtoValidator>().As<IValidator<SurveyForCreateDto>>().InstancePerLifetimeScope();
            #endregion
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
