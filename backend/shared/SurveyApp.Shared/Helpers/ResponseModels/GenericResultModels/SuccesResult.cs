namespace SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels
{
    public class SuccesResult : Result, IResult
    {
        public SuccesResult(string message) : base(true, message)
        {

        }
        public SuccesResult() : base(true)
        {

        }

    }
}
