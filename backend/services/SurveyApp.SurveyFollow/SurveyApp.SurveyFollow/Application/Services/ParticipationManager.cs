using FluentValidation;
using MassTransit;
using SurveyApp.Shared.Constant;
using SurveyApp.Shared.Events;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyFollow.Application.Dto_s;
using SurveyApp.SurveyFollow.Domain.Entities;
using SurveyApp.SurveyFollow.Infrastructure.Repositories.Interfaces;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.SurveyFollow.Application.Services
{
    public class ParticipationManager : IParticipationService
    {
        private readonly IParticipationRepository _participationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ParticipationForCreateDto> _validator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<ParticipationManager> _log;

        public ParticipationManager(
            IParticipationRepository participationRepository,
            IUnitOfWork unitOfWork,
            IValidator<ParticipationForCreateDto> validator,
            IPublishEndpoint publishEndpoint,
            ILogger<ParticipationManager> log)
        {
            _participationRepository = participationRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _publishEndpoint = publishEndpoint;
            _log = log;
        }

        public async Task<IResult> SubmitParticipation(ParticipationForCreateDto dto, int userId)
        {
            try
            {
                // Validasyon
                var validationResult = await _validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                    return new ErrorResult(string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));

                // Tekrar doldurma engeli
                var alreadyParticipated = await _participationRepository.HasUserParticipatedAsync(dto.SurveyId, userId);
                if (alreadyParticipated)
                    return new ErrorResult("Bu anketi daha önce doldurdunuz.");

                // Kaydet
                var participation = new Participation
                {
                    SurveyId = dto.SurveyId,
                    UserId = userId,
                    ParticipationDate = DateTime.UtcNow,
                    Answers = dto.Answers.Select(a => new Answer
                    {
                        QuestionId = a.QuestionId,
                        OptionId = a.OptionId,
                        TextAnswer = a.TextAnswer
                    }).ToList()
                };

                await _participationRepository.AddAsync(participation);
                await _unitOfWork.CommitAsync();

                await _publishEndpoint.Publish<ISurveySubmittedEvent>(new
                {
                    SurveyId = dto.SurveyId,
                    SubmittedAt = DateTime.UtcNow,
                    Answers = dto.Answers.Select(a => new
                    {
                        a.QuestionId,
                        a.QuestionText,   
                        a.OptionId,
                        a.OptionText,     
                        a.TextAnswer
                    })
                });


                return new SuccesResult(Messages.SurveySuccessCompleted);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "SubmitParticipation hata: {Message}", ex.Message);
                throw;
            }
        }
    }
}
