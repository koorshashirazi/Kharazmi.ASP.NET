using System.Net;
using IdentityModel.Client;

namespace Kharazmi.Net.Http.OAuth2
{
    public class TokenValidationResult
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public TokenResponse TokenResponse { get; set; }
    }
}