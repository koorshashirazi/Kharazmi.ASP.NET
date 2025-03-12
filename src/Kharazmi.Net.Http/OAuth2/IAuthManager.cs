using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel.Client;

namespace Kharazmi.Net.Http.OAuth2
{
    public interface IAuthManager
    {
        string CurrentAccessToken { get; }
        string CurrentRefreshToken { get; }
        IdentityResult GetCurrentUser { get; }
        bool IsAuthenticated { get; }

        string CreateTempState();

        TempStateValidationResult SignInTempState(string state, bool isPersistent);

        TempStateValidationResult CreateRequestUrl(string state);

        TempStateValidationResult TempStateValidation(string state);

        void SignOutTempState();

        TokenValidationResult RequestTokenResponse(string codeResponse);

        IdentityResult ValidateResponse(TokenResponse response); //, string nonce

        IdentityResult RequestRefreshToken(string refreshToken);

        TempStateValidationResult GetTempState();

        IdentityResult UpdateAuthentication();

        List<Claim> GetUserClaims();

        IdentityResult RequestUserInfo(string accessToken);

        IdentityResult RequestIntrospection(string accessToken);

        IdentityResult RevokeToken();

        string GetFromQueryString(string key);

        string GetFromBody(string key);

        Dictionary<string, string> DecodeJsonWebToken(string token);

        void RedirectTo(string redirectUrl);

        void RedirectTo(string redirectUrl, bool endResponse = false);

        void CallAuthServer(string url);

        IdentityResult SignIn(IEnumerable<Claim> claims, AuthenticationConfig authenticationConfig,
            JwtClaimModelTypes nameType, JwtClaimModelTypes roleType);

        IdentityResult SignIn(ClaimsPrincipal user, AuthenticationConfig authenticationConfig,
            JwtClaimModelTypes nameType, JwtClaimModelTypes roleType);

        IdentityResult SignOut();

        void SignOutAuthentication();
    }
}