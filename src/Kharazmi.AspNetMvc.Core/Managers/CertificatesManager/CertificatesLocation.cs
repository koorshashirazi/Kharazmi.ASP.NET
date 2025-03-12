using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

#pragma warning disable 1591

namespace Kharazmi.AspNetMvc.Core.Managers.CertificatesManager
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CertificatesLocation
    {
        private readonly StoreLocation _location;

        public CertificatesLocation(StoreLocation location)
        {
            _location = location;
        }

        public CertificatesName My => new CertificatesName(_location, StoreName.My);

        public CertificatesName AddressBook => new CertificatesName(_location, StoreName.AddressBook);

        public CertificatesName TrustedPeople => new CertificatesName(_location, StoreName.TrustedPeople);

        public CertificatesName TrustedPublisher => new CertificatesName(_location, StoreName.TrustedPublisher);

        public CertificatesName CertificateAuthority => new CertificatesName(_location, StoreName.CertificateAuthority);
    }
}