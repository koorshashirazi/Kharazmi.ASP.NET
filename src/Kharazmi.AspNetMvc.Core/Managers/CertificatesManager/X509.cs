using System.Security.Cryptography.X509Certificates;

#pragma warning disable 1591

namespace Kharazmi.AspNetMvc.Core.Managers.CertificatesManager
{
    public static class X509
    {
        public static CertificatesLocation CurrentUser => new CertificatesLocation(StoreLocation.CurrentUser);

        public static CertificatesLocation LocalMachine => new CertificatesLocation(StoreLocation.LocalMachine);
    }
}