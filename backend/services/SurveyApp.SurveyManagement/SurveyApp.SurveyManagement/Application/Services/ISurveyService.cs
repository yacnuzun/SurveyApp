using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using SurveyApp.SurveyManagement.Application.Dto_s;
using SurveyApp.SurveyManagement.Domain.Entities;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.SurveyManagement.Application.Services
{
    public interface ISurveyService
    {
        Task<IResult> CreateComplexSurvey(SurveyForCreateDto surveyDto, int adminId);
        Task<IDataResult<List<Survey>>> GetAllActiveSurveys();
    }
}
