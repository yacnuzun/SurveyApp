namespace SurveyApp.ReportingService.Application.Dto_s
{

    public class QuestionReportDto
    {
        public string QuestionText { get; set; }
        public List<string> Labels { get; set; } = new();
        public List<int> Data { get; set; } = new();

    }
}
