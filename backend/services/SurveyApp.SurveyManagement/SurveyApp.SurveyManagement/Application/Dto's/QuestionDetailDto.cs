namespace SurveyApp.SurveyManagement.Application.Dto_s
{
    public class QuestionDetailDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Type { get; set; } // Survey.ts'deki enum ile eşleşecek
        public List<OptionDetailDto> Options { get; set; }
    }
}
