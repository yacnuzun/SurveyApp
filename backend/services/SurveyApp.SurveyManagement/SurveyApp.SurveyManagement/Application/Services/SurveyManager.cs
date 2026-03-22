using FluentValidation;
using SurveyApp.Shared.Constant;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyManagement.Application.Dto_s;
using SurveyApp.SurveyManagement.Domain.Entities;
using SurveyApp.SurveyManagement.Infrastructure.Repositories.Interfaces;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.SurveyManagement.Application.Services
{

    public class SurveyManager : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SurveyForCreateDto> _validator;
        private readonly ILogger<SurveyManager> _log;

        public SurveyManager(ISurveyRepository surveyRepository,
            IUnitOfWork unitOfWork,
            IValidator<SurveyForCreateDto> validator,
            ILogger<SurveyManager> logger)
        {
            _surveyRepository = surveyRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _log = logger;
        }

        public async Task<IResult> CreateComplexSurvey(SurveyForCreateDto surveyDto, int adminId)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(surveyDto);
                if (!validationResult.IsValid)
                {
                    // Hataları birleştirip geri dönüyoruz
                    var errorMessages = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return new ErrorResult(errorMessages);
                }

                var survey = new Survey
                {
                    Title = surveyDto.Title,
                    Description = surveyDto.Description,
                    StartDate = surveyDto.StartDate.ToUniversalTime(),
                    EndDate = surveyDto.EndDate.ToUniversalTime(),
                    CreatedBy = adminId,
                    IsActive = true,
                    Questions = surveyDto.Questions.Select(q => new Question
                    {
                        Text = q.Text,
                        Type = q.Type,
                        Order = q.Order,
                        Options = q.Options?.Select(o => new Option
                        {
                            Text = o.Text,
                            Order = o.Order
                        }).ToList()
                    }).ToList()
                };

                await _surveyRepository.AddAsync(survey);
                await _unitOfWork.CommitAsync();
                return new SuccesResult(Messages.SurveySuccessCreated);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
            
        }

        public async Task<IDataResult<List<Survey>>> GetAllActiveSurveys()
        {
            var result = await _surveyRepository.ListAsync(s => s.IsActive && s.EndDate > DateTime.UtcNow);
            return new SuccessDataResult<List<Survey>>(result.ToList());
        }

        public async Task<IDataResult<SurveyDetailDto>> GetSurveywithId(int id)
        {
            var survey = await _surveyRepository.GetWithQuestionsAndOptionsAsync(id);
            if (survey == null)
                return new ErrorDataResult<SurveyDetailDto>("Anket bulunamadı.");

            // Entity -> DTO Dönüşümü (Manuel Mapping)
            var surveyDto = new SurveyDetailDto
            {
                Id = survey.Id,
                Title = survey.Title,
                Description = survey.Description,
                Questions = survey.Questions.Select(q => new QuestionDetailDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Type = (int)q.Type,
                    Options = q.Options?.Select(o => new OptionDetailDto
                    {
                        Id = o.Id,
                        Text = o.Text
                    }).ToList()
                }).ToList()
            };
            return new SuccessDataResult<SurveyDetailDto>(surveyDto);
        }
    }
}
