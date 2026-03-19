namespace SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels
{
    public interface IResult
    {
        public bool Success { get; }
        public string Message { get; }
    }
}
