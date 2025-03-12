namespace Mvc.Utility.Core.Managers.ResultManager
{
    public interface IResult
    {
        bool IsFailure { get; }
        bool IsSuccess { get; }
    }
}