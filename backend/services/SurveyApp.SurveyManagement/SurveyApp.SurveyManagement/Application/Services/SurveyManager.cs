using FluentValidation;
using SurveyApp.Shared.Constant;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyManagement.Application.Dto_s;
using SurveyApp.SurveyManagement.Domain.Entities;
using SurveyApp.SurveyManagement.Domain.Enums;
using SurveyApp.SurveyManagement.Infrastructure.Repositories.Implemantations;
using SurveyApp.SurveyManagement.Infrastructure.Repositories.Interfaces;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.SurveyManagement.Application.Services
{

    public class SurveyManager : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ISurveyQuestionRepository _surveyQuestionRepository;
        private readonly ISurveyUserRepository _surveyUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SurveyForCreateDto> _validator;
        private readonly ILogger<SurveyManager> _log;

        public SurveyManager(
            ISurveyRepository surveyRepository,
            ISurveyQuestionRepository surveyQuestionRepository,
            ISurveyUserRepository surveyUserRepository,
            IUnitOfWork unitOfWork,
            IValidator<SurveyForCreateDto> validator,
            ILogger<SurveyManager> log)
        {
            _surveyRepository = surveyRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _surveyUserRepository = surveyUserRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _log = log;
        }

        public async Task<IResult> CreateAsync(SurveyForCreateDto dto, int adminId)
        {
            try
            {
                var validation = await _validator.ValidateAsync(dto);
                if (!validation.IsValid)
                    return new ErrorResult(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));

                var survey = new Survey
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    StartDate = dto.StartDate.ToUniversalTime(),
                    EndDate = dto.EndDate.ToUniversalTime(),
                    CreatedByAdminId = adminId,
                    IsActive = true,
                    SurveyQuestions = dto.QuestionIds.Select((qId, i) => new SurveyQuestion
                    {
                        QuestionId = qId,
                        Order = i + 1
                    }).ToList(),
                    AssignedUsers = dto.AssignedUserIds.Select(uid => new SurveyUser
                    {
                        UserId = uid
                    }).ToList()
                };

                await _surveyRepository.AddAsync(survey);
                await _unitOfWork.CommitAsync();
                return new SuccesResult(Messages.SurveySuccessCreated);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Survey create hata: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IResult> UpdateAsync(SurveyForUpdateDto dto)
        {
            var survey = await _surveyRepository.GetWithQuestionsAsync(dto.Id);
            if (survey == null) return new ErrorResult("Anket bulunamadı.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                survey.Title = dto.Title;
                survey.Description = dto.Description;
                survey.StartDate = dto.StartDate.ToUniversalTime();
                survey.EndDate = dto.EndDate.ToUniversalTime();
                survey.IsActive = dto.IsActive;

                // Navigation üzerinden değil, direkt DB'den sil
                var existingQuestions = await _surveyQuestionRepository
                    .ListAsync(sq => sq.SurveyId == dto.Id);
                foreach (var sq in existingQuestions)
                    _surveyQuestionRepository.Delete(sq);

                var existingUsers = await _surveyUserRepository
                    .ListAsync(su => su.SurveyId == dto.Id);
                foreach (var su in existingUsers)
                    _surveyUserRepository.Delete(su);

                await _unitOfWork.CommitAsync(); // silmeleri yaz

                // Yenileri ekle
                foreach (var (qId, i) in dto.QuestionIds.Select((id, i) => (id, i)))
                    await _surveyQuestionRepository.AddAsync(new SurveyQuestion
                    {
                        SurveyId = survey.Id,
                        QuestionId = qId,
                        Order = i + 1
                    });

                foreach (var uid in dto.AssignedUserIds)
                    await _surveyUserRepository.AddAsync(new SurveyUser
                    {
                        SurveyId = survey.Id,
                        UserId = uid
                    });

                _surveyRepository.Update(survey);
                await _unitOfWork.CommitAsync(); // eklemeleri yaz

                await _unitOfWork.CommitTransactionAsync();
                return new SuccesResult(Messages.SurveyUpdated);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var survey = await _surveyRepository.GetAsync(s => s.Id == id);
            if (survey == null) return new ErrorResult("Anket bulunamadı.");
            _surveyRepository.Delete(survey);
            await _unitOfWork.CommitAsync();
            return new SuccesResult(Messages.SurveyDeleted);
        }

        public async Task<IResult> ToggleActiveAsync(int id)
        {
            var survey = await _surveyRepository.GetAsync(s => s.Id == id);
            if (survey == null) return new ErrorResult("Anket bulunamadı.");
            survey.IsActive = !survey.IsActive;
            _surveyRepository.Update(survey);
            await _unitOfWork.CommitAsync();
            return new SuccesResult(survey.IsActive ? "Anket aktifleştirildi." : "Anket pasife alındı.");
        }

        public async Task<IDataResult<List<SurveyDetailDto>>> GetAllActiveSurveys()
        {
            var surveys = await _surveyRepository.GetAllWithDetailsAsync();
            return new SuccessDataResult<List<SurveyDetailDto>>(
                surveys.Select(MapToDetailDto).ToList()
            );

        }

        public async Task<IDataResult<SurveyDetailDto>> GetSurveywithId(int id)
        {
            var survey = await _surveyRepository.GetWithQuestionsAsync(id);
            if (survey == null) return new ErrorDataResult<SurveyDetailDto>("Anket bulunamadı.");
            return new SuccessDataResult<SurveyDetailDto>(MapToDetailDto(survey));
        }

        public async Task<IDataResult<List<SurveyDetailDto>>> GetAssignedSurveysForUser(int userId)
        {
            var surveys = await _surveyRepository.GetAssignedSurveysForUserAsync(userId);
            return new SuccessDataResult<List<SurveyDetailDto>>(surveys.Select(MapToDetailDto).ToList());
        }

        private SurveyDetailDto MapToDetailDto(Survey s) => new()
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            StartDate = s.StartDate,    
            EndDate = s.EndDate,        
            IsActive = s.IsActive,
            AssignedUserIds = s.AssignedUsers.Select(u => u.UserId).ToList(),
            Questions = s.SurveyQuestions.OrderBy(sq => sq.Order).Select(sq => new QuestionDetailDto
            {
                Id = sq.Question.Id,
                Text = sq.Question.Text,
                Type = sq.Question.Type,
                Options = sq.Question.AnswerTemplate?.Options.Select(o => new TemplateOptionDetailDto
                {
                    Id = o.Id,
                    Text = o.Text
                }).ToList() ?? new List<TemplateOptionDetailDto>()
            }).ToList()
        };
    }
    public class AnswerTemplateManager : IAnswerTemplateService
    {
        private readonly IAnswerTemplateRepository _templateRepository;
        private readonly ITemplateOptionRepository _optionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AnswerTemplateForCreateDto> _validator;
        private readonly ILogger<AnswerTemplateManager> _log;

        public AnswerTemplateManager(
            IAnswerTemplateRepository templateRepository,
            ITemplateOptionRepository optionRepository,
            IUnitOfWork unitOfWork,
            IValidator<AnswerTemplateForCreateDto> validator,
            ILogger<AnswerTemplateManager> log)
        {
            _templateRepository = templateRepository;
            _optionRepository = optionRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _log = log;
        }

        public async Task<IDataResult<List<AnswerTemplateDetailDto>>> GetAllAsync()
        {
            var templates = await _templateRepository.GetAllWithOptionsAsync();
            return new SuccessDataResult<List<AnswerTemplateDetailDto>>(templates.Select(MapToDto).ToList());
        }

        public async Task<IDataResult<AnswerTemplateDetailDto>> GetByIdAsync(int id)
        {
            var template = await _templateRepository.GetWithOptionsAsync(id);
            if (template == null)
                return new ErrorDataResult<AnswerTemplateDetailDto>("Şablon bulunamadı.");
            return new SuccessDataResult<AnswerTemplateDetailDto>(MapToDto(template));
        }

        public async Task<IResult> CreateAsync(AnswerTemplateForCreateDto dto)
        {
            try
            {
                var validation = await _validator.ValidateAsync(dto);
                if (!validation.IsValid)
                    return new ErrorResult(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));

                var template = new AnswerTemplate
                {
                    Name = dto.Name,
                    OptionCount = dto.Options.Count,
                    Options = dto.Options.Select((o, i) => new TemplateOption
                    {
                        Text = o.Text,
                        Order = i + 1
                    }).ToList()
                };

                await _templateRepository.AddAsync(template);
                await _unitOfWork.CommitAsync();
                return new SuccesResult(Messages.AnswerTemplateCreated);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "AnswerTemplate create hata: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IResult> UpdateAsync(AnswerTemplateForUpdateDto dto)
        {
            var template = await _templateRepository.GetWithOptionsAsync(dto.Id);
            if (template == null) return new ErrorResult("Şablon bulunamadı.");

            if (dto.Options.Count < 2 || dto.Options.Count > 4)
                return new ErrorResult("Şablon 2 ile 4 arasında seçenek içermelidir.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Önce eski seçenekleri sil
                foreach (var opt in template.Options.ToList())
                    _optionRepository.Delete(opt);

                await _unitOfWork.CommitAsync(); // silmeleri DB'ye yaz

                // Sonra yenileri ekle
                template.Name = dto.Name;
                template.OptionCount = dto.Options.Count;
                template.Options = dto.Options.Select((o, i) => new TemplateOption
                {
                    Text = o.Text,
                    Order = i + 1,
                    AnswerTemplateId = template.Id
                }).ToList();

                _templateRepository.Update(template);
                await _unitOfWork.CommitAsync(); // güncellemeleri DB'ye yaz

                await _unitOfWork.CommitTransactionAsync();
                return new SuccesResult(Messages.AnswerTemplateUpdated);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<IResult> DeleteAsync(int id)
        {
            var template = await _templateRepository.GetWithOptionsAsync(id);
            if (template == null) return new ErrorResult("Şablon bulunamadı.");

            if (template.Questions.Any())
                return new ErrorResult("Bu şablon aktif sorularda kullanılıyor, silinemez.");

            _templateRepository.Delete(template);
            await _unitOfWork.CommitAsync();
            return new SuccesResult(Messages.AnswerTemplateDeleted);
        }

        private AnswerTemplateDetailDto MapToDto(AnswerTemplate t) => new()
        {
            Id = t.Id,
            Name = t.Name,
            OptionCount = t.OptionCount,
            Options = t.Options.Select(o => new TemplateOptionDetailDto
            {
                Id = o.Id,
                Text = o.Text,
                Order = o.Order
            }).ToList()
        };
    }
    public class QuestionManager : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerTemplateRepository _templateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<QuestionForCreateDto> _validator;
        private readonly ILogger<QuestionManager> _log;

        public QuestionManager(
            IQuestionRepository questionRepository,
            IAnswerTemplateRepository templateRepository,
            IUnitOfWork unitOfWork,
            IValidator<QuestionForCreateDto> validator,
            ILogger<QuestionManager> log)
        {
            _questionRepository = questionRepository;
            _templateRepository = templateRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _log = log;
        }

        public async Task<IDataResult<List<QuestionDetailDto>>> GetAllAsync()
        {
            var questions = await _questionRepository.GetAllWithTemplatesAsync();
            return new SuccessDataResult<List<QuestionDetailDto>>(questions.Select(MapToDto).ToList());
        }

        public async Task<IResult> CreateAsync(QuestionForCreateDto dto)
        {
            try
            {
                var validation = await _validator.ValidateAsync(dto);
                if (!validation.IsValid)
                    return new ErrorResult(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));

                if (dto.AnswerTemplateId.HasValue)
                {
                    var template = await _templateRepository.GetAsync(t => t.Id == dto.AnswerTemplateId.Value);
                    if (template == null)
                        return new ErrorResult("Seçilen şablon bulunamadı.");
                }

                var question = new Question
                {
                    Text = dto.Text,
                    Type = dto.Type,
                    AnswerTemplateId = dto.AnswerTemplateId,
                    CreatedAt = DateTime.UtcNow
                };

                await _questionRepository.AddAsync(question);
                await _unitOfWork.CommitAsync();
                return new SuccesResult(Messages.QuestionCreated);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Question create hata: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IResult> UpdateAsync(QuestionForUpdateDto dto)
        {
            var question = await _questionRepository.GetAsync(q => q.Id == dto.Id);
            if (question == null) return new ErrorResult("Soru bulunamadı.");

            question.Text = dto.Text;
            question.Type = dto.Type;
            question.AnswerTemplateId = dto.Type == QuestionType.Text ? null : dto.AnswerTemplateId;

            _questionRepository.Update(question);
            await _unitOfWork.CommitAsync();
            return new SuccesResult(Messages.QuestionUpdated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var question = await _questionRepository.GetAsync(q => q.Id == id);
            if (question == null) return new ErrorResult("Soru bulunamadı.");

            var inUse = await _questionRepository.GetAsync(q =>
                q.Id == id && q.SurveyQuestions.Any());
            if (inUse != null)
                return new ErrorResult("Bu soru aktif anketlerde kullanılıyor, silinemez.");

            _questionRepository.Delete(question);
            await _unitOfWork.CommitAsync();
            return new SuccesResult(Messages.QuestionDeleted);
        }

        private QuestionDetailDto MapToDto(Question q) => new()
        {
            Id = q.Id,
            Text = q.Text,
            Type = q.Type,
            AnswerTemplateId = q.AnswerTemplateId,
            AnswerTemplateName = q.AnswerTemplate?.Name,
            Options = q.AnswerTemplate?.Options.Select(o => new TemplateOptionDetailDto
            {
                Id = o.Id,
                Text = o.Text,
                Order = o.Order
            }).ToList() ?? new()
        };
    }
}
