using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using SurveyApp.SurveyManagement.Application.Dto_s;
using SurveyApp.SurveyManagement.Domain.Entities;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.SurveyManagement.Application.Services
{
    public interface IAnswerTemplateService
    {
        Task<IDataResult<List<AnswerTemplateDetailDto>>> GetAllAsync();
        Task<IDataResult<AnswerTemplateDetailDto>> GetByIdAsync(int id);
        Task<IResult> CreateAsync(AnswerTemplateForCreateDto dto);
        Task<IResult> UpdateAsync(AnswerTemplateForUpdateDto dto);
        Task<IResult> DeleteAsync(int id);
    }
    public interface IQuestionService
    {
        Task<IDataResult<List<QuestionDetailDto>>> GetAllAsync();
        Task<IResult> CreateAsync(QuestionForCreateDto dto);
        Task<IResult> UpdateAsync(QuestionForUpdateDto dto);
        Task<IResult> DeleteAsync(int id);
    }
    public interface ISurveyService
    {
        Task<IDataResult<List<SurveyDetailDto>>> GetAllSurveys();
        Task<IResult> CreateAsync(SurveyForCreateDto dto, int adminId);
        Task<IResult> UpdateAsync(SurveyForUpdateDto dto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> ToggleActiveAsync(int id);
        Task<IDataResult<SurveyDetailDto>> GetSurveywithId(int id);
        Task<IDataResult<List<SurveyDetailDto>>> GetAssignedSurveysForUser(int userId);
        Task<IDataResult<List<SurveyDetailDto>>> GetAllActiveSurveys();
    }

}
