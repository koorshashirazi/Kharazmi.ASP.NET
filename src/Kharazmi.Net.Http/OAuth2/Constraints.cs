using Mvc.Utility.Core.Constraints;
using Mvc.Utility.Core.Extensions;
using Mvc.Utility.Core.Helpers;
using System.Collections.Generic;

namespace Kharazmi.Net.Http.OAuth2
{
    public static class Constraints
    {
        public const string ID_TOKEN = "id_token";
        public const string REFRESH_TOKEN = "refresh_token";
        public const string ACCESS_TOKEN = "access_token";
        public const string EXPIRES_AT = "expires_at";
        public const string OPEN_ID_CONNECT_AUTHENTICATION_TYPE = "oidc";
        public const string AUTHENTICATION_COOKIE = "Cookies";
        public const string AUTHENTICATION_TEMP_STATE_COOKIE = "TempStateCookie";
        public const string CODE = "code";
        public const string AUTHORIZATION_CODE_GRANT_TYPE = "authorization_code";
        public const string TOKEN_REFRESH_GRANT_TYPE = "refresh_code";
        public const string STATE = "state";
        public const string CODE_CHALLENGE_METHOD = "S256";
        public const string NONCE = "nonce";
        public const string ERROR = "error";
        public const string ERROR_DESCRIPTION = "error_description";
        public const string SUCCESS = "success";
        public const string IS_PERSISTENT = "isPersistent";
        public const string XsrfKey = "XsrfId";
        public const string FORM_CONTENT_TYPE = "application/x-www-form-urlencoded";

        private static readonly string codeVerifier = HashHelper.CreateCodeVerifier(32);

        /// <summary>
        ///     Default Client configuration
        /// </summary>
        public static ClientModel ClientModel => new ClientModel
        {
            BaseAddress = BASE_ADDRESS,
            AuthorizeEndpoint = AUTHORIZE_ENDPOINT,
            LogoutEndpoint = LOGOUT_ENDPOINT,
            TokenEndpoint = TOKEN_ENDPOINT,
            UserInfoEndpoint = USER_INFO_ENDPOINT,
            TokenValidationEndpoint = DISCOVERY_ENDPOINT,
            TokenRevocationEndpoint = TOKEN_REVOCATION_ENDPOINT,
            TokenIntrospectionEndpoint = TOKEN_INTROSPECTION_ENDPOINT,
            ClientId = CLIENT_ID,
            ClientName = CLIENT_NAME,
            ClientSecret = CLIENT_SECRET,
            Issuer = ISSUER,
            AlgorithmType = ALGORITHM_YPE,
            AcrValues = ACR_VALUES,
            ResponseType = CODE,
            Scope = SCOPES,
            RedirectUri = REDIRECT_URI,
            ResponseMode = QUERY_RESPONSE_MODE,
            PostLogoutRedirectUri = POST_LOGOUT_REDIRECT_URI,
            CodeVerifier = codeVerifier,
            CodeChallengeMethod = CODE_CHALLENGE_METHOD,
            CodeChallenge = codeVerifier.ToChallengeCode(),
            FilterClaimTypes = FilterClaimTypes,
            AuthenticationConfig = new AuthenticationConfig
            {
                IsPersistent = true,
                AllowRefresh = true,
                RedirectUri = "~/",
                ExpiresUtc = 15.TimeOffset(ExpiresTimeType.Minutes)
            },
            NameType = JwtClaimModelTypes.Name,
            RoleType = JwtClaimModelTypes.Role
        };

        public static List<string> FilterClaimTypes { get; set; } = new List<string>();
        //{
        //        "iss",
        //        "exp",
        //        "nbf",
        //        "aud",
        //        "nonce",
        //        "c_hash",
        //        "iat",
        //        "auth_time"
        //};

        #region Information ueber identity server

        public const string BASE_ADDRESS = "https://localhost:6655";
        //public const string BASE_ADDRESS = "https://localhost:44377";

        public const string AUTHORIZE_ENDPOINT = "/connect/authorize";
        public const string LOGOUT_ENDPOINT = "/connect/endsession";
        public const string TOKEN_ENDPOINT = "/connect/token";
        public const string USER_INFO_ENDPOINT = "/connect/userinfo";
        public const string DISCOVERY_ENDPOINT = "/.well-known/openid-configuration/jwks";
        public const string TOKEN_REVOCATION_ENDPOINT = "/connect/revocation";
        public const string TOKEN_INTROSPECTION_ENDPOINT = "/connect/introspect";

        #endregion Information ueber identity server

        #region Information ueber client

        public const string CLIENT_ID = "335f7e5d426d4786830a6a8629f7f8cd";
        //public const string CLIENT_ID = "df7d64a2-756d-48d5-8c34-ed55eb05f58b";

        public const string CLIENT_NAME = "HMP Client";

        public const string CLIENT_SECRET = "0805aeddcf754e3ab73b24d51f3a9a56";
        //public const string CLIENT_SECRET = "59cbe148-1c2b-4989-ba9f-dbf7c2145522";

        private const string ISSUER = "https://localhost:6655";
        public const string ACR_VALUES = "acr_value_test";
        public const string ALGORITHM_YPE = "RS256";
        public const string RESPONSE_TYPE = "code";
        public const string SCOPES = "openid profile";
        public const string AUTHORIZE_SCOPE = "openid";
        public const string USER_INFO_SCOPE = "profile";
        public const string FORM_POST_RESPONSE_MODE = "form_post";
        public const string QUERY_RESPONSE_MODE = "query";
        public const string KTY_VALID = "RSA";

        // TODO Change to new url
        public const string REDIRECT_URI = "http://localhost/HMP.IdentityClient.TestApp/oauth2/login.aspx";

        //public const string REDIRECT_URI = "https://lmsng-int.es.corpintra.net/LMSTESTHMP/Login.aspx";

        public const string POST_LOGOUT_REDIRECT_URI = "http://localhost/HMP.IdentityClient.TestApp/oauth2/logout.aspx";
        //public const string POST_LOGOUT_REDIRECT_URI = "https://lmsng-int.es.corpintra.net/LMSTESTHMP/Logout.aspx";

        #endregion Information ueber client
    }
}