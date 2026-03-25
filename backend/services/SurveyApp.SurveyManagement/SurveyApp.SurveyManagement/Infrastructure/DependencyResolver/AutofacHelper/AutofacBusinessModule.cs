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
            builder.RegisterType<AnswerTemplateManager>().As<IAnswerTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionManager>().As<IQuestionService>().InstancePerLifetimeScope();
            #endregion

            builder.RegisterType<EfUnitOfWork<SurveyManagementDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();

            #region repos
            builder.RegisterType<SurveyRepository>().As<ISurveyRepository>();
            builder.RegisterType<QuestionRepository>().As<IQuestionRepository>();
            builder.RegisterType<AnswerTemplateRepository>().As<IAnswerTemplateRepository>();
            builder.RegisterType<SurveyUserRepository>().As<ISurveyUserRepository>();
            builder.RegisterType<SurveyQuestionRepository>().As<ISurveyQuestionRepository>();
            builder.RegisterType<TemplateOptionRepository>().As<ITemplateOptionRepository>();
            #endregion

            #region validators
            builder.RegisterType<SurveyForCreateDtoValidator>().As<IValidator<SurveyForCreateDto>>().InstancePerLifetimeScope();
            builder.RegisterType<AnswerTemplateForCreateDtoValidator>().As<IValidator<AnswerTemplateForCreateDto>>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionForCreateDtoValidator>().As<IValidator<QuestionForCreateDto>>().InstancePerLifetimeScope();
            
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
