namespace SurveyApp.ReportingService.Application.Dto_s
{

    public class QuestionReportDto
    {
        public string QuestionText { get; set; }
        // Frontend'deki Chart.js'in beklediği format:
        public List<string> Labels { get; set; } // ["Evet", "Hayır"]
        public List<int> Data { get; set; }      // [15, 5]
    }
}
