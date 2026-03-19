namespace SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels
{
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
