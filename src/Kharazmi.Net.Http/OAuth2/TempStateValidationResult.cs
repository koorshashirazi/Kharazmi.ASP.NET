
using IdentityModel.Client;

namespace Kharazmi.Net.Http.OAuth2
{
    public class TempStateValidationResult
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public TempState TempState { get; set; }
        public string RedirectUri { get; set; }
        public AuthorizeResponse AuthorizeResponse { get; set; }
    }
}