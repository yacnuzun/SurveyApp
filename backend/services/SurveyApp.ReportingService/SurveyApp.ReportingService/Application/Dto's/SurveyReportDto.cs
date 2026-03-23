namespace SurveyApp.ReportingService.Application.Dto_s
{
    public class SurveyReportDto
    {
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public int TotalParticipants { get; set; }
        public List<QuestionReportDto> QuestionResults { get; set; }
    }
}
