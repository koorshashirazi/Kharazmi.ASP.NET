using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Mvc.Utility.Core.Helpers;
using Newtonsoft.Json.Linq;
using JsonWebKeySet = IdentityModel.Jwk.JsonWebKeySet;

namespace Kharazmi.Net.Http.OAuth2
{
    /// <summary>
    ///     AuthorizationCodeFlow
    /// </summary>
    public class AuthManager : IAuthManager
    {
        public AuthManager(ClientModel clientModel, HttpContextBase httpContextBase)
        {
            if (httpContextBase == null)
                throw ExceptionHelper.ThrowException<NullReferenceException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource),
                        nameof(NullReferenceException),
                        nameof(HttpContextBase)), nameof(HttpContextBase));

            if (clientModel == null)
                throw ExceptionHelper.ThrowException<ArgumentNullException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(NullReferenceException),
                        nameof(HttpContext)), nameof(HttpContext));

            HttpContextBase = httpContextBase;
            ClientModel = clientModel;

            Request = HttpContextBase.Request;
            Response = HttpContextBase.Response;
        }

        public ClientModel ClientModel { get; }
        public HttpContextBase HttpContextBase { get; }
        public HttpRequestBase Request { get; }
        public HttpResponseBase Response { get; }

        public string CurrentExpireToken =>
            IsAuthenticated ? GetCurrentUser.User.FindFirst(Constraints.EXPIRES_AT)?.Value : string.Empty;

        public string CurrentIdToken =>
            IsAuthenticated ? GetCurrentUser.User.FindFirst(Constraints.ID_TOKEN)?.Value : string.Empty;

        public HttpClient Client => DiscoverySingleton.Instance.HttpClient(ClientModel.BaseAddress);

        private DiscoveryResponse DiscoveryResponse =>
            DiscoverySingleton.Instance.GetDiscoveryResponse(ClientModel.BaseAddress);

        public bool IsAuthenticated
        {
            get
            {
                var user = Request.GetOwinContext().Authentication.User;
                return user != null &&
                       user.Identity.IsAuthenticated;
            }
        }

        public IdentityResult GetCurrentUser
        {
            get
            {
                if (!IsAuthenticated)
                    return new IdentityResult
                    {
                        ErrorMessage = ShareResource.InvalidCredentialException,
                        User = null
                    };

                return new IdentityResult
                {
                    Success = true,
                    User = Request.GetOwinContext().Authentication.User
                };
            }
        }

        public string CurrentAccessToken => IsAuthenticated
            ? GetCurrentUser.User.FindFirst(Constraints.ACCESS_TOKEN)?.Value
            : string.Empty;

        public string CurrentRefreshToken => IsAuthenticated
            ? GetCurrentUser.User.FindFirst(Constraints.REFRESH_TOKEN)?.Value
            : string.Empty;

        public List<Claim> GetUserClaims()
        {
            var user = GetCurrentUser.User;

            return user?.Claims.ToList();
        }

        /// <summary>
        ///     Authentication Request Generate
        ///     response_type:
        ///     This value MUST be “code”.
        ///     This requests that both an Access Token and an ID Token be returned from the Token Endpoint in exchange for the
        ///     code value returned from the Authorization Endpoint.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>AuthorizationUrlString</returns>
        public TempStateValidationResult CreateRequestUrl(string state)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(state))
                    return new TempStateValidationResult
                    {
                        ErrorMessage = ResourceHelper.ReadResourceValue(typeof(ShareResource),
                            nameof(ArgumentNullException),
                            nameof(state))
                    };

                var request = new RequestUrl(DiscoveryResponse.AuthorizeEndpoint);

                var url = request.CreateAuthorizeUrl(
                    ClientModel.ClientId,
                    ClientModel.ResponseType,
                    ClientModel.Scope,
                    ClientModel.RedirectUri,
                    responseMode: ClientModel.ResponseMode,
                    state: state
                    //codeChallengeMethod: ClientModel.CodeChallengeMethod,
                    //codeChallenge: ClientModel.CodeChallenge
                    );

                return new TempStateValidationResult
                {
                    Success = true,
                    RedirectUri = url,
                    AuthorizeResponse = new AuthorizeResponse(url)
                };
            }
            catch (Exception e)
            {
                return new TempStateValidationResult
                {
                    ErrorMessage = e.Message
                };
            }
        }

        public void CallAuthServer(string url)
        {
            HttpContextBase.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = url });
            Response.StatusCode = 401;
            Response.End();
        }

        /// <summary>
        ///     if the state parameter is present in the Authorization Request. Clients MUST verify that
        ///     the state value is equal to the value of state parameter in the Authorization Request.
        ///     Error code:
        ///     access_denied
        ///     invalid_request
        ///     invalid_scope
        /// </summary>
        /// <param name="stateResponse"></param>
        /// <returns></returns>
        public TempStateValidationResult TempStateValidation(string stateResponse)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(stateResponse))
                    return new TempStateValidationResult
                    {
                        ErrorMessage = ResourceHelper.ReadResourceValue(typeof(ShareResource),
                            nameof(ArgumentNullException),
                            nameof(Constraints.STATE))
                    };

                var result = GetTempState();

                var error = GetFromQueryString(Constraints.ERROR);
                var errorDescription = GetFromQueryString(Constraints.ERROR_DESCRIPTION);

                if (!stateResponse.Equals(result.TempState.State, StringComparison.Ordinal))
                    return new TempStateValidationResult
                    {
                        ErrorMessage = error,
                        ErrorDescription = errorDescription,
                        TempState = new TempState
                        {
                            State = result.TempState.State
                        }
                    };

                return new TempStateValidationResult
                {
                    Success = true,
                    TempState = new TempState
                    {
                        State = result.TempState.State
                    }
                };
            }
            catch (Exception e)
            {
                return new TempStateValidationResult
                {
                    ErrorMessage = e.Message
                };
            }
            finally
            {
                SignOutTempState();
            }
        }

        /// <summary>
        ///     After the authentication response the Client Application makes a Token Request using the Authorization Code to
        ///     obtain an ID and Access token from the Token Endpoint.
        ///     A Client starts a Token Request by presenting its Authorization Grant (in the form of an Authorization Code) to
        ///     the Token Endpoint using the grant_type value authorization_code, as described in Section 4.1.3 of OAuth 2.0 RFC
        ///     6749.
        ///     =====================================================================================
        ///     Once the Access Token has been obtained it can be used to make calls to the API by passing it as a Bearer Token in
        ///     the Authorization header of the HTTP request:
        ///     =====================================================================================
        ///     grant_type REQUIRED
        ///     This value MUST be “authorization_code”.
        ///     code REQUIRED
        ///     Code which has been issued in the authentication response.
        ///     redirect_uri REQUIRED
        ///     Redirection URI that was used in the corresponding Authorization Request (URL encoded).
        ///     code_verifier OPTIONAL REQUIRED
        ///     if PKCE is used and code_challenge was sent in the corresponding Authorization Request.
        ///     =====================================================================================
        ///     The Client receives a response with the following parameters as described in Section 4.1.4 of OAuth 2.0 RFC 6749.
        ///     The response SHOULD be encoded using UTF-8. Parameter
        ///     access_token REQUIRED Access Token for a resource server for example the UserInfo Endpoint.
        ///     token_type REQUIRED OAuth 2.0 Token Type value.
        ///     The value MUST be “Bearer”, as specified in OAuth 2.0 Bearer Token Usage RFC 6750, for Clients using this subset.
        ///     id_token REQUIRED ID Token as defined in chapter ID Token verification.
        ///     expires_in REQUIRED Expiration time of the Access Token in seconds since the response was generated.
        ///     refresh_token REQUIRED Refresh Token to receive a new access token without user interaction.
        /// </summary>
        /// <param name="codeResponse"></param>
        /// <returns>TokenValidationResult</returns>
        public TokenValidationResult RequestTokenResponse(string codeResponse)
        {
            try
            {
                var code = string.IsNullOrWhiteSpace(codeResponse)
                    ? GetFromQueryString(Constraints.CODE)
                    : codeResponse;

                if (string.IsNullOrWhiteSpace(code))
                    return new TokenValidationResult
                    {
                        Success = false,
                        ErrorMessage = ShareResource.ErrorInvalidCode
                    };

                // Token Request
                var response = Client.RequestAuthorizationCodeTokenAsync(
                    new AuthorizationCodeTokenRequest
                    {
                        Address = DiscoveryResponse.TokenEndpoint,
                        ClientId = ClientModel.ClientId,
                        ClientSecret = ClientModel.ClientSecret,
                        Code = code,
                        RedirectUri = ClientModel.RedirectUri
                        //GrantType = OidcConstants.GrantTypes.AuthorizationCode,
                        //CodeVerifier = ClientModel.CodeVerifier
                    }).ConfigureAwait(false).GetAwaiter().GetResult();

                // Successful Token Response
                if (response.IsError)
                    return new TokenValidationResult
                    {
                        ErrorMessage = response.Error,
                        HttpStatusCode = response.HttpStatusCode
                    };

                return new TokenValidationResult
                {
                    Success = true,
                    TokenResponse = response
                };
            }
            catch (Exception e)
            {
                return new TokenValidationResult
                {
                    ErrorMessage = e.Message
                };
            }
        }

        public IdentityResult ValidateResponse(TokenResponse tokenResponse)
        {
            try
            {
                if (tokenResponse == null)
                    return new IdentityResult
                    {
                        ErrorMessage = ResourceHelper.ReadResourceValue(typeof(ShareResource),
                            nameof(ArgumentNullException),
                            nameof(tokenResponse))
                    };

                if (string.IsNullOrWhiteSpace(tokenResponse.IdentityToken))
                    return new IdentityResult { ErrorMessage = ShareResource.ErrorInvalidToken };

                var idTokenClaims = ValidateIdentityToken(tokenResponse);

                if (idTokenClaims == null) return new IdentityResult { ErrorMessage = ShareResource.ErrorInvalidToken };

                var userClaims = RequestUserInfo(tokenResponse.AccessToken).UserClaims;

                var claimsList = new ClaimsIdentity()
                    .AddIdTokenClaim(tokenResponse)
                    .AddAccessTokenToClaims(tokenResponse)
                    .AddRefreshTokenToClaims(tokenResponse)
                    .AddExpiresAtToClaims(tokenResponse)
                    .AddUserInfoToClaims(tokenResponse)
                    .AddExtraClaims(idTokenClaims)
                    .RemoveClaims(ClientModel.FilterClaimTypes)
                    .Claims;

                claimsList = claimsList.Distinct(new ClaimComparer());

                return new IdentityResult
                {
                    Success = true,
                    User = new ClaimsPrincipal(new ClaimsIdentity(claimsList,
                        Constraints.AUTHENTICATION_COOKIE)),
                    UserClaims = userClaims,
                    IdentityToken = tokenResponse.IdentityToken,
                    AccessToken = tokenResponse.AccessToken,
                    RefreshToken = tokenResponse.RefreshToken,
                    AccessTokenExpiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    ErrorMessage = e.Message,
                    User = null,
                    UserClaims = null
                };
            }
        }

        public IdentityResult RequestRefreshToken(string refreshToken)
        {
            try
            {
                var response = Client.RequestRefreshTokenAsync(
                    new RefreshTokenRequest
                    {
                        RefreshToken = refreshToken,
                        Address = DiscoveryResponse.TokenEndpoint,
                        ClientId = ClientModel.ClientId,
                        ClientSecret = ClientModel.ClientSecret
                    }).ConfigureAwait(false).GetAwaiter().GetResult();

                if (response.IsError)
                    return new IdentityResult
                    {
                        ErrorMessage = ResourceHelper.ReadResourceValue(
                            typeof(ShareResource),
                            nameof(TokenResponse),
                            response.Error,
                            response.HttpStatusCode)
                    };

                return new IdentityResult
                {
                    Success = true,
                    IdentityToken = response.IdentityToken,
                    AccessToken = response.AccessToken,
                    RefreshToken = response.RefreshToken,
                    ExpiresIn = response.ExpiresIn
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    ErrorMessage = e.Message
                };
            }
        }

        /// <summary>
        ///     The Client can then use the Access Token to access protected resources at Resource Servers on behalf of the user,
        ///     in this case the UserInfo Endpoint
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns>ClaimsUserInfo</returns>
        public IdentityResult RequestUserInfo(string accessToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                    return new IdentityResult
                    {
                        ErrorMessage = ResourceHelper.ReadResourceValue(
                            typeof(ShareResource),
                            nameof(ArgumentNullException),
                            nameof(Constraints.ACCESS_TOKEN),
                            nameof(Constraints.ACCESS_TOKEN))
                    };

                var userInfo = Client.GetUserInfoAsync(
                    new UserInfoRequest
                    {
                        Token = accessToken,
                        Address = DiscoveryResponse.UserInfoEndpoint
                    }).ConfigureAwait(false).GetAwaiter().GetResult();

                if (userInfo.IsError)
                    return new IdentityResult
                    {
                        ErrorMessage = ResourceHelper.ReadResourceValue(
                            typeof(ShareResource),
                            nameof(UserInfoResponse),
                            userInfo.Error,
                            userInfo.HttpStatusCode)
                    };

                return new IdentityResult
                {
                    UserClaims = userInfo.Claims.ToList(),
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    ErrorMessage = e.Message
                };
            }
        }

        /// <summary>
        ///     The server still does not support this case
        ///     If the AccessTokenType is active on the server side, you can send an introspection request to the server
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public IdentityResult RequestIntrospection(string accessToken)
        {
            try
            {
                var result = Client.IntrospectTokenAsync(
                    new TokenIntrospectionRequest
                    {
                        Address = DiscoveryResponse.IntrospectionEndpoint,
                        ClientId = ClientModel.ClientId,
                        ClientSecret = ClientModel.ClientSecret,
                        TokenTypeHint = Constraints.ACCESS_TOKEN,
                        Token = accessToken
                    }).ConfigureAwait(false).GetAwaiter().GetResult();

                if (result.IsError)
                    return new IdentityResult
                    {
                        ErrorMessage = result.Error
                    };

                if (result.IsActive)
                    return new IdentityResult
                    {
                        UserClaims = result.Claims.ToList()
                    };

                return new IdentityResult
                {
                    ErrorMessage = "Token is not active"
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    ErrorMessage = e.Message
                };
            }
        }

        /// <summary>
        ///     the current access token one, if it has expired. Updates the authentication.
        ///     Return new access token
        /// </summary>
        /// <returns>AccessToken</returns>
        public IdentityResult UpdateAuthentication()
        {
            try
            {
                var accessToken = CurrentAccessToken;
                var expiresAt = CurrentExpireToken;

                if (string.IsNullOrWhiteSpace(expiresAt) ||
                    DateTime.Parse(expiresAt).AddSeconds(-60).ToUniversalTime() < DateTime.UtcNow)
                {
                    var identityResult = RenewTokens();
                    if (!identityResult.Success) return identityResult;

                    accessToken = identityResult.AccessToken;

                    if (string.IsNullOrWhiteSpace(accessToken))
                        return new IdentityResult
                        {
                            Success = false,
                            ErrorMessage = ShareResource.InvalidCredentialException
                        };
                    SignOutAuthentication();
                    SignIn(identityResult.UserClaims, ClientModel.AuthenticationConfig, ClientModel.NameType,
                        ClientModel.RoleType);

                    return identityResult;
                }

                if (string.IsNullOrWhiteSpace(accessToken))
                    return new IdentityResult
                    {
                        Success = false,
                        ErrorMessage = ShareResource.InvalidCredentialException
                    };
                SignOutAuthentication();
                SignIn(GetCurrentUser.User, ClientModel.AuthenticationConfig, ClientModel.NameType,
                    ClientModel.RoleType);

                return new IdentityResult
                {
                    Success = true,
                    AccessToken = accessToken,
                    User = GetCurrentUser.User,
                    UserClaims = GetUserClaims().ToList()
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public IdentityResult RevokeToken()
        {
            try
            {
                RevokeAccessToken();
                RevokeRefreshToken();
                return new IdentityResult
                {
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public string CreateTempState()
        {
            var state = Guid.NewGuid().ToString("N");
            return state;
        }

        /// <summary>
        ///     State:
        ///     Opaque value used to maintain state between the request and the callback.
        ///     Typically, Cross-Site Request Forgery (CSRF, XSRF) mitigation is done by cryptographically binding the value of
        ///     this parameter with a browser cookie.
        ///     Nonce :
        ///     String value used to associate a Client session with an ID Token, and to mitigate replay attacks.
        ///     The value is passed through unmodified from the Authorization Request to the ID Token.
        ///     Sufficient entropy MUST be present in the nonce values used to prevent attackers from guessing values.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isPersistent"></param>
        public TempStateValidationResult SignInTempState(string state, bool isPersistent = false)
        {
            try
            {
                SignOutAuthentication();

                var message = string.Empty;

                if (string.IsNullOrWhiteSpace(state))
                    message = ResourceHelper.ReadResourceValue(
                        typeof(ShareResource),
                        nameof(ArgumentNullException),
                        nameof(state));

                if (!string.IsNullOrWhiteSpace(message))
                    return new TempStateValidationResult
                    {
                        ErrorMessage = message
                    };

                if (!string.IsNullOrWhiteSpace(message))
                    return new TempStateValidationResult
                    {
                        ErrorMessage = message
                    };

                var tempId = new ClaimsIdentity(Constraints.AUTHENTICATION_TEMP_STATE_COOKIE);
                tempId.AddClaim(new Claim(Constraints.STATE, state));

                Request.GetOwinContext().Authentication
                    .SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, tempId);

                return new TempStateValidationResult
                {
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new TempStateValidationResult
                {
                    ErrorMessage = e.Message
                };
            }
        }

        public IdentityResult SignIn(IEnumerable<Claim> claims, AuthenticationConfig authenticationConfig,
            JwtClaimModelTypes nameType, JwtClaimModelTypes roleType)
        {
            try
            {
                if (authenticationConfig == null)
                    return new IdentityResult
                    {
                        Success = false,
                        ErrorMessage = ResourceHelper.ReadResourceValue(typeof(ShareResource),
                            nameof(ArgumentNullException), nameof(AuthenticationConfig))
                    };

                var authProperty = new AuthenticationProperties
                {
                    IsPersistent = authenticationConfig.IsPersistent,
                    ExpiresUtc = authenticationConfig.ExpiresUtc,
                    AllowRefresh = authenticationConfig.AllowRefresh
                };

                var claimsIdentity =
                    new ClaimsIdentity(claims, Constraints.AUTHENTICATION_COOKIE, nameType.Value, roleType.Value);
                HttpContextBase.GetOwinContext().Authentication.SignIn(authProperty, claimsIdentity);
                return new IdentityResult
                {
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public IdentityResult SignIn(ClaimsPrincipal user, AuthenticationConfig authenticationConfig,
            JwtClaimModelTypes nameType, JwtClaimModelTypes roleType)
        {
            try
            {
                if (authenticationConfig == null)
                    return new IdentityResult
                    {
                        Success = false,
                        ErrorMessage = ResourceHelper.ReadResourceValue(typeof(ShareResource),
                            nameof(ArgumentNullException), nameof(AuthenticationConfig))
                    };

                if (user == null)
                    return new IdentityResult
                    {
                        Success = false,
                        ErrorMessage = new ArgumentNullException(nameof(ClaimsIdentity)).Message
                    };
                var authProperty = new AuthenticationProperties
                {
                    IsPersistent = authenticationConfig.IsPersistent,
                    ExpiresUtc = authenticationConfig.ExpiresUtc,
                    AllowRefresh = authenticationConfig.AllowRefresh
                };
                var claimsIdentity = new ClaimsIdentity(user.Claims, user.Identity.AuthenticationType, nameType.Value,
                    roleType.Value);

                HttpContextBase.GetOwinContext().Authentication.SignIn(authProperty, claimsIdentity);

                return new IdentityResult
                {
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public void SignOutTempState()
        {
            Request.GetOwinContext().Authentication.SignOut(Constraints.AUTHENTICATION_TEMP_STATE_COOKIE);
        }

        public IdentityResult SignOut()
        {
            try
            {
                var identityResult = RevokeToken();

                if (!identityResult.Success) return identityResult;

                HttpContextBase
                    .GetOwinContext()
                    .Authentication
                    .SignOut(Constraints.AUTHENTICATION_COOKIE,
                        Constraints.AUTHENTICATION_TEMP_STATE_COOKIE);

                var request =
                    new RequestUrl(ClientModel.LogoutEndpoint).CreateEndSessionUrl(CurrentIdToken,
                        ClientModel.PostLogoutRedirectUri);

                RedirectTo(request, true);

                return new IdentityResult
                {
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public void SignOutAuthentication()
        {
            HttpContextBase.GetOwinContext().Authentication.SignOut(Constraints.AUTHENTICATION_COOKIE);
        }

        public void RedirectTo(string redirectUrl)
        {
            Response.Redirect(string.IsNullOrWhiteSpace(redirectUrl) ? "/" : redirectUrl);
        }

        public void RedirectTo(string redirectUrl, bool endResponse)
        {
            Response.Redirect(string.IsNullOrWhiteSpace(redirectUrl) ? "/" : redirectUrl, endResponse);
        }

        public Dictionary<string, string> DecodeJsonWebToken(string token)
        {
            if (!token.Contains("."))
                return new Dictionary<string, string>
                {
                    {
                        "AccessToken", token
                    }
                };

            var parts = token.Split('.');
            var header = Encoding.UTF8.GetString(Base64Url.Decode(parts[0]));
            var payload = Encoding.UTF8.GetString(Base64Url.Decode(parts[1]));

            var jsonWebToken = new Dictionary<string, string>();

            var headerObject = JObject.Parse(header);
            var alg = headerObject.GetValue("alg").Value<string>();
            var kid = headerObject.GetValue("kid").Value<string>();

            jsonWebToken.Add(nameof(alg), alg);
            jsonWebToken.Add(nameof(kid), kid);

            var payloadObject = JObject.Parse(payload);

            var iss = payloadObject.GetValue("iss").Value<string>();
            var sub = payloadObject.GetValue("sub").Value<string>();
            var aud = payloadObject.GetValue("aud").Value<string>();
            var exp = payloadObject.GetValue("exp").Value<string>();
            var iat = payloadObject.GetValue("iat").Value<string>();
            var acr = payloadObject.GetValue("acr").Value<string>();

            jsonWebToken.Add(nameof(iss), iss);
            jsonWebToken.Add(nameof(sub), sub);
            jsonWebToken.Add(nameof(aud), aud);
            jsonWebToken.Add(nameof(exp), exp);
            jsonWebToken.Add(nameof(iat), iat);
            jsonWebToken.Add(nameof(acr), acr);

            return jsonWebToken;
        }

        public TempStateValidationResult GetTempState()
        {
            try
            {
                var authenticateCookieResult = Request
                    .GetOwinContext()
                    .Authentication
                    .AuthenticateAsync(Constraints.AUTHENTICATION_TEMP_STATE_COOKIE)
                    .Result;

                if (authenticateCookieResult == null)
                    return new TempStateValidationResult
                    {
                        ErrorMessage = ShareResource.ErrorInvalidState
                    };

                var state = authenticateCookieResult.Identity.FindFirst(Constraints.STATE)?.Value;

                if (string.IsNullOrWhiteSpace(state))
                    return new TempStateValidationResult
                    {
                        ErrorMessage = ShareResource.ErrorInvalidState
                    };

                return new TempStateValidationResult
                {
                    Success = true,
                    TempState = new TempState
                    {
                        State = state,
                        Nonce = string.Empty
                    }
                };
            }
            catch (Exception e)
            {
                return new TempStateValidationResult
                {
                    ErrorMessage = e.Message
                };
            }
        }

        public string GetFromQueryString(string key)
        {
            return Request.QueryString[key];
        }

        public string GetFromBody(string key)
        {
            return Request.Form[key];
        }

        protected virtual bool ValidateCodeHash(string cHash, string code)
        {
            using (var sha = SHA256.Create())
            {
                var codeHash = sha.ComputeHash(Encoding.ASCII.GetBytes(code));
                var leftBytes = new byte[16];
                Array.Copy(codeHash, leftBytes, 16);

                var codeHashB64 = Base64Url.Encode(leftBytes);

                return string.Equals(cHash, codeHashB64, StringComparison.Ordinal);
            }
        }

        /// <summary>
        ///     ID Token Validation or Token Response Validation
        /// </summary>
        /// <param name="tokenResponse"></param>
        /// <returns></returns>
        protected virtual List<Claim> ValidateIdentityToken(TokenResponse tokenResponse)
        {
            if (tokenResponse == null) throw new ArgumentNullException(nameof(tokenResponse));
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var tokenDecoder = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenDecoder.ReadToken(tokenResponse.IdentityToken) as JwtSecurityToken;
            if (jwtSecurityToken == null) return null;

            var issuer = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "iss")?.Value;
            var audience = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "aud")?.Value;
            var acr = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "acr")?.Value;

            if (issuer != ClientModel.Issuer || audience != ClientModel.ClientId /*||acr != ClientModel.AcrValues*/
            ) return null;

            var keys = new List<SecurityKey>();
            var webKeys = GetJsonWebKeySet().Keys;

            foreach (var webKey in webKeys)
            {
                if (string.IsNullOrWhiteSpace(webKey.Kty) || webKey.Kty != Constraints.KTY_VALID) continue;

                if (string.IsNullOrWhiteSpace(webKey.E)) continue;

                if (string.IsNullOrWhiteSpace(webKey.N)) continue;

                var e = Base64Url.Decode(webKey.E);
                var n = Base64Url.Decode(webKey.N);

                var key = new RsaSecurityKey(new RSAParameters { Modulus = n, Exponent = e })
                {
                    KeyId = webKey.Kid
                };

                keys.Add(key);
            }

            var parameter = new TokenValidationParameters
            {
                ValidIssuer = ClientModel.Issuer,
                ValidAudience = ClientModel.ClientId,
                IssuerSigningKeys = keys
            };

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var handler = new JwtSecurityTokenHandler();

            SecurityToken token;
            try
            {
                return handler.ValidateToken(jwtSecurityToken.RawData, parameter,
                    out token).Claims.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected virtual JsonWebKeySet GetJsonWebKeySet()
        {
            //var disco = Client
            //            .GetStringAsync(requestUri: ClientModel.TokenValidationEndpoint)
            //            .ConfigureAwait(false).GetAwaiter().GetResult();

            //return new JsonWebKeySet(json: disco);

            return DiscoveryResponse.KeySet;
        }

        protected virtual IdentityResult RenewTokens()
        {
            try
            {
                // refresh the tokens
                var identityResult = RequestRefreshToken(CurrentRefreshToken);

                if (!identityResult.Success) return identityResult;

                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(identityResult.ExpiresIn);

                // get authenticate result, containing the current principal & properties
                var currentAuthenticateResult = HttpContextBase.GetOwinContext()
                    .Authentication
                    .AuthenticateAsync(Constraints.AUTHENTICATION_COOKIE)
                    .ConfigureAwait(false).GetAwaiter().GetResult();

                var claims = currentAuthenticateResult.Identity.Claims.ToList();

                var result = from c in claims
                             where c.Type != Constraints.ACCESS_TOKEN &&
                                   c.Type != Constraints.REFRESH_TOKEN &&
                                   c.Type != Constraints.EXPIRES_AT
                             select c;

                claims = result.ToList();

                claims.Add(new Claim(Constraints.ACCESS_TOKEN, identityResult.AccessToken));
                claims.Add(new Claim(Constraints.EXPIRES_AT, expiresAt.ToString("o", CultureInfo.InvariantCulture)));
                claims.Add(new Claim(Constraints.REFRESH_TOKEN, identityResult.RefreshToken));

                return new IdentityResult
                {
                    Success = true,
                    AccessToken = identityResult.AccessToken,
                    UserClaims = claims
                };
            }
            catch (Exception e)
            {
                return new IdentityResult
                {
                    ErrorMessage = e.Message
                };
            }
        }

        protected virtual TokenRevocationResponse RevokeAccessToken()
        {
            if (string.IsNullOrWhiteSpace(CurrentAccessToken)) return null;

            var response = Client.RevokeTokenAsync(
                new TokenRevocationRequest
                {
                    Address = ClientModel.TokenRevocationEndpoint,
                    ClientId = ClientModel.ClientId,
                    ClientSecret = ClientModel.ClientSecret,
                    TokenTypeHint = Constraints.ACCESS_TOKEN,
                    Token = CurrentAccessToken
                }).ConfigureAwait(false).GetAwaiter().GetResult();

            if (response.IsError)
                throw ExceptionHelper.ThrowException<InvalidCredentialException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource),
                        nameof(TokenRevocationResponse),
                        response.Error, response.HttpStatusCode));

            return response;
        }

        protected virtual TokenRevocationResponse RevokeRefreshToken()
        {
            if (string.IsNullOrWhiteSpace(CurrentRefreshToken)) return null;

            var response = Client.RevokeTokenAsync(
                new TokenRevocationRequest
                {
                    Address = ClientModel.TokenRevocationEndpoint,
                    ClientId = ClientModel.ClientId,
                    ClientSecret = ClientModel.ClientSecret,
                    TokenTypeHint = Constraints.REFRESH_TOKEN,
                    Token = CurrentRefreshToken
                }).ConfigureAwait(false).GetAwaiter().GetResult();

            if (response.IsError)
                throw ExceptionHelper.ThrowException<InvalidCredentialException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource),
                        nameof(TokenRevocationResponse),
                        response.Error, response.HttpStatusCode));

            return response;
        }
    }
}