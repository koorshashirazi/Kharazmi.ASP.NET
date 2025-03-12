namespace Kharazmi.AspNetMvc.Core.Managers.Session
{
    public interface ISessionStoreManager : ISessionLifeTimeProvider
    {
        object this[string index] { get; set; }
        object Get(string key);
        T Get<T>(string key) where T : class;
        void Remove(string key);
        void RemoveAll();
        void Add(string key, object value);
    }
}