using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

#pragma warning disable 1591

namespace Kharazmi.AspNetMvc.Core.Managers.CertificatesManager
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CertificatesName
    {
        private readonly StoreLocation _location;
        private readonly StoreName _name;

        public CertificatesName(StoreLocation location, StoreName name)
        {
            _location = location;
            _name = name;
        }

        public CertificatesFinder Thumbprint => new CertificatesFinder(_location, _name, X509FindType.FindByThumbprint);

        public CertificatesFinder SubjectDistinguishedName =>
            new CertificatesFinder(_location, _name, X509FindType.FindBySubjectDistinguishedName);

        public CertificatesFinder SerialNumber =>
            new CertificatesFinder(_location, _name, X509FindType.FindBySerialNumber);

        public CertificatesFinder IssuerName => new CertificatesFinder(_location, _name, X509FindType.FindByIssuerName);
    }
}