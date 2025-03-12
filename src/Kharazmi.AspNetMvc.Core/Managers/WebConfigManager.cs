using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Dynamic;

namespace Kharazmi.AspNetMvc.Core.Managers
{
    public sealed class WebConfigManager : DynamicObject
    {
        private static readonly Lazy<WebConfigManager> LAZY =
            new Lazy<WebConfigManager>(() => new WebConfigManager());

        private readonly NameValueCollection _items;

        private WebConfigManager()
        {
            _items = ConfigurationManager.AppSettings;
        }

        public static WebConfigManager Instance => LAZY.Value;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _items[binder.Name];
            return result != null;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _items[binder.Name] = value?.ToString();
            return true;
        }
    }

    public sealed class WebConfigManager<T> : DynamicObject
    {
        private static readonly Lazy<WebConfigManager<T>> LAZY =
            new Lazy<WebConfigManager<T>>(() => new WebConfigManager<T>());

        private readonly NameValueCollection _items;

        private WebConfigManager()
        {
            _items = ConfigurationManager.AppSettings;
        }

        public static WebConfigManager<T> Instance => LAZY.Value;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Convert.ChangeType(_items[binder.Name], typeof(T));
            return result != null;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _items[binder.Name] = value?.ToString();
            return true;
        }
    }
}