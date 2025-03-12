namespace Mvc.Utility.Core.Managers.Session
{
    public interface ISessionLifeTimeProvider
    {
        void ResetSession();
        string GetCurrentSessionId();
        bool IsNewSession();
        void SetNewTimeOut(int minute);
    }
}