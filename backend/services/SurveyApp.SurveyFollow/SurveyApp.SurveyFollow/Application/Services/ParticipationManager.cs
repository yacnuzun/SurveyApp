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
        private readonly ILogger<ParticipationManager> _log;
        private readonly IValidator<ParticipationForCreateDto> _participationValidator;
        private readonly IPublishEndpoint _publishEndpoint;

        public ParticipationManager(IParticipationRepository participationRepository, IUnitOfWork unitOfWork, IValidator<ParticipationForCreateDto> participationValidator, ILogger<ParticipationManager> log, IPublishEndpoint publishEndpoint)
        {
            _participationRepository = participationRepository;
            _unitOfWork = unitOfWork;
            _participationValidator = participationValidator;
            _log = log;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IResult> SubmitParticipation(ParticipationForCreateDto dto, int userId)
        {
            try
            {
                var validationResult = await _participationValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                    return new ErrorResult(string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));

                var alreadyParticipated = await _participationRepository.GetAsync(p => p.UserId == userId && p.SurveyId == dto.SurveyId);
                if (alreadyParticipated is not null)
                    return new ErrorResult("Bu anketi daha önce doldurdunuz.");

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
                    //ParticipantId = dto.UserId,
                    SubmittedAt = DateTime.UtcNow,
                    Answers = dto.Answers // AnswerMessageDto listesi
                });

                return new SuccesResult(Messages.SurveySuccessCompleted);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
            
        }
    }
}
