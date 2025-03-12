using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Kharazmi.Net.Http.OAuth2
{
    public class IdentityResult
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public ClaimsPrincipal User { get; set; } = null;
        public AuthenticationProperties AuthenticationProperties { get; set; } = null;
        public List<Claim> UserClaims { get; set; } = null;
        public string AccessToken { get; set; }
        public string IdentityToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}