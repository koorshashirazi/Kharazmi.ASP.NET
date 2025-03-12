using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

#pragma warning disable 1591

namespace Mvc.Utility.Core.Managers.CertificatesManager
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CertificatesFinder
    {
        private readonly X509FindType _findType;
        private readonly StoreLocation _location;
        private readonly StoreName _name;

        public CertificatesFinder(StoreLocation location, StoreName name, X509FindType findType)
        {
            _location = location;
            _name = name;
            _findType = findType;
        }

        public IEnumerable<X509Certificate2> Find(object findValue, bool validOnly = true)
        {
#if NET452
            var store = new X509Store(_name, _location);
            store.Open(OpenFlags.ReadOnly);

            try
            {
                var certColl = store.Certificates.Find(_findType, findValue, validOnly);
                store.Close();
                return certColl.Cast<X509Certificate2>();
            }
            finally
            {
                store.Close();
            }
#else
            var store = new X509Store(_name, _location);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                var certColl = store.Certificates.Find(_findType, findValue, validOnly);

                return certColl.Cast<X509Certificate2>();
            }
            finally
            {
                store.Close();
            }

#endif
        }
    }
}