using System;
using System.Web;
using System.Web.SessionState;

namespace Kharazmi.AspNetMvc.Core.Managers.Session
{
    public class SessionStoreManager : ManagerBase, ISessionStoreManager
    {
        private readonly HttpSessionState _session;

        public SessionStoreManager(HttpContextBase httpContextBase)
        {
            if (httpContextBase == null) throw new ArgumentNullException(nameof(httpContextBase));

            _session = httpContextBase.ApplicationInstance.Session;
            _session.Add(_session.SessionID, "KKKK");
        }

        public object this[string key]
        {
            get
            {
                return Execute(
                    () =>
                    {
                        if (string.IsNullOrWhiteSpace(key))
                            throw new ArgumentException(@"Value cannot be null or whitespace.", nameof(key));

                        return _session[key];
                    });
            }

            set
            {
                Execute(
                    () =>
                    {
                        if (string.IsNullOrWhiteSpace(key))
                            throw new ArgumentException(@"Value cannot be null or whitespace.", nameof(key));

                        _session[key] = value ?? throw new ArgumentNullException(nameof(value));
                    });
            }
        }

        public object Get(string key)
        {
            return Execute(
                () =>
                {
                    if (string.IsNullOrWhiteSpace(key))
                        throw new ArgumentException(@"Value cannot be null or whitespace.", nameof(key));

                    return _session[key];
                });
        }

        public T Get<T>(string key) where T : class
        {
            return Execute(
                () =>
                {
                    if (string.IsNullOrWhiteSpace(key))
                        throw new ArgumentException(@"Value cannot be null or whitespace.", nameof(key));

                    return _session[key] as T;
                });
        }

        public void Remove(string key)
        {
            Execute(
                () =>
                {
                    if (string.IsNullOrWhiteSpace(key))
                        throw new ArgumentException(@"Value cannot be null or whitespace.", nameof(key));

                    _session.Remove(key);
                });
        }

        public void RemoveAll()
        {
            Execute(() => { _session.RemoveAll(); });
        }

        public void Add(string key, object value)
        {
            Execute(
                () =>
                {
                    if (string.IsNullOrWhiteSpace(key))
                        throw new ArgumentException(@"Value cannot be null or whitespace.", nameof(key));

                    _session[key] = value ?? throw new ArgumentNullException(nameof(value));
                });
        }

        public void ResetSession()
        {
            Execute(
                () =>
                {
                    _session.Clear();
                    _session.RemoveAll();
                    _session.Abandon();
                });
        }

        public string GetCurrentSessionId()
        {
            return Execute(() => _session.SessionID);
        }

        public bool IsNewSession()
        {
            return Execute(() => _session.IsNewSession);
        }

        public void SetNewTimeOut(int minute)
        {
            Execute(
                () =>
                {
                    if (minute <= 0 || minute == 20) return;

                    _session.Clear();
                    _session.RemoveAll();
                    _session.Timeout = minute;
                });
        }
    }
}