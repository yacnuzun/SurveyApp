using SurveyApp.SurveyFollow.Application.Dto_s;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.SurveyFollow.Application.Services
{
    public interface IParticipationService
    {
        Task<IResult> SubmitParticipation(ParticipationForCreateDto dto, int userId);
    }
}
