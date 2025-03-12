using Mvc.Utility.Core.Helpers;
using Mvc.Utility.Core.Managers.UrlManager;
using System;
using System.Collections.Generic;

namespace Kharazmi.Net.Http.OAuth2
{
    public class ClientModel
    {
        private string _authorizeEndpoint;
        private string _baseAddress;
        private string _tokenValidationEndpoint;
        private string _logoutEndpoint;
        private string _postLogoutRedirectUri;
        private string _redirectUri;
        private string _tokenEndpoint;
        private string _tokenIntrospectionEndpoint;
        private string _tokenRevocationEndpoint;
        private string _userInfoEndpoint;

        public string BaseAddress
        {
            get { return _baseAddress; }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw ExceptionHelper.ThrowException<ArgumentNullException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                            nameof(BaseAddress)), nameof(BaseAddress));

                if (!Url.IsValid(value))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrl",
                            nameof(BaseAddress)),
                        nameof(BaseAddress));

                _baseAddress = value;
            }
        }

        public string AuthorizeEndpoint
        {
            get { return _authorizeEndpoint; }

            set
            {
                if (string.IsNullOrWhiteSpace(BaseAddress))
                    throw ExceptionHelper.ThrowException<ArgumentNullException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                            nameof(BaseAddress)), nameof(BaseAddress));

                if (!string.IsNullOrWhiteSpace(value)) _authorizeEndpoint = Url.Combine(BaseAddress, value);

                if (!Url.IsValid(_authorizeEndpoint))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrlEndpoint",
                            nameof(AuthorizeEndpoint)), nameof(AuthorizeEndpoint));
            }
        }

        public string LogoutEndpoint
        {
            get { return _logoutEndpoint; }

            set
            {
                if (string.IsNullOrWhiteSpace(BaseAddress))
                    throw ExceptionHelper.ThrowException<ArgumentNullException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                            nameof(BaseAddress)), nameof(BaseAddress));

                if (!string.IsNullOrWhiteSpace(value)) _logoutEndpoint = Url.Combine(BaseAddress, value);

                if (!Url.IsValid(_logoutEndpoint))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrlEndpoint",
                            nameof(LogoutEndpoint)), nameof(LogoutEndpoint));
            }
        }

        public string TokenEndpoint
        {
            get { return _tokenEndpoint; }

            set
            {
                if (string.IsNullOrWhiteSpace(BaseAddress))
                    throw ExceptionHelper.ThrowException<ArgumentNullException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                            nameof(BaseAddress)), nameof(BaseAddress));

                if (!string.IsNullOrWhiteSpace(value)) _tokenEndpoint = Url.Combine(BaseAddress, value);

                if (!Url.IsValid(_tokenEndpoint))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrlEndpoint",
                            nameof(TokenEndpoint)), nameof(TokenEndpoint));
            }
        }

        public string UserInfoEndpoint
        {
            get { return _userInfoEndpoint; }

            set
            {
                if (string.IsNullOrWhiteSpace(BaseAddress))
                    throw ExceptionHelper.ThrowException<ArgumentNullException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                            nameof(BaseAddress)), nameof(BaseAddress));

                if (!string.IsNullOrWhiteSpace(value)) _userInfoEndpoint = Url.Combine(BaseAddress, value);

                if (!Url.IsValid(_userInfoEndpoint))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrlEndpoint",
                            nameof(UserInfoEndpoint)), nameof(UserInfoEndpoint));
            }
        }

        public string TokenValidationEndpoint
        {
            get { return _tokenValidationEndpoint; }

            set
            {
                if (string.IsNullOrWhiteSpace(BaseAddress))
                    throw ExceptionHelper.ThrowException<ArgumentNullException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                            nameof(BaseAddress)), nameof(BaseAddress));

                if (!string.IsNullOrWhiteSpace(value))
                    _tokenValidationEndpoint = Url.Combine(BaseAddress, value);

                if (!Url.IsValid(_tokenValidationEndpoint))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrlEndpoint",
                            nameof(TokenValidationEndpoint)), nameof(TokenValidationEndpoint));
            }
        }

        public string TokenRevocationEndpoint
        {
            get { return _tokenRevocationEndpoint; }

            set
            {
                if (string.IsNullOrWhiteSpace(BaseAddress))
                    throw ExceptionHelper.ThrowException<ArgumentNullException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                            nameof(BaseAddress)), nameof(BaseAddress));

                if (!string.IsNullOrWhiteSpace(value)) _tokenRevocationEndpoint = Url.Combine(BaseAddress, value);

                if (!Url.IsValid(_tokenRevocationEndpoint))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrlEndpoint",
                            nameof(TokenRevocationEndpoint)), nameof(TokenRevocationEndpoint));
            }
        }

        public string TokenIntrospectionEndpoint
        {
            get { return _tokenIntrospectionEndpoint; }

            set
            {
                if (string.IsNullOrWhiteSpace(BaseAddress))
                    throw ExceptionHelper.ThrowException<ArgumentNullException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                            nameof(BaseAddress)), nameof(BaseAddress));

                if (!string.IsNullOrWhiteSpace(value)) _tokenIntrospectionEndpoint = Url.Combine(BaseAddress, value);

                if (!Url.IsValid(_tokenIntrospectionEndpoint))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrlEndpoint",
                            nameof(_tokenIntrospectionEndpoint)), nameof(_tokenIntrospectionEndpoint));
            }
        }

        public string RedirectUri
        {
            get { return _redirectUri; }

            set
            {
                if (!Url.IsValid(value))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrlEndpoint",
                            nameof(RedirectUri)), nameof(RedirectUri));

                _redirectUri = value;
            }
        }

        public string PostLogoutRedirectUri
        {
            get { return _postLogoutRedirectUri; }

            set
            {
                if (!Url.IsValid(value))
                    throw ExceptionHelper.ThrowException<InvalidOperationException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResource), "ErrorInvalidUrlEndpoint",
                            nameof(PostLogoutRedirectUri)), nameof(PostLogoutRedirectUri));

                _postLogoutRedirectUri = value;
            }
        }

        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
        public string Issuer { get; set; }
        public string AcrValues { get; set; }
        public string AlgorithmType { get; set; }
        public string ResponseType { get; set; }
        public string Scope { get; set; }
        public string ResponseMode { get; set; }
        public string CodeChallenge { get; set; }
        public string CodeChallengeMethod { get; set; }
        public string CodeVerifier { get; set; }
        public List<string> FilterClaimTypes { get; set; }
        public AuthenticationConfig AuthenticationConfig { get; set; }
        public JwtClaimModelTypes NameType { get; set; } = JwtClaimModelTypes.Subject;
        public JwtClaimModelTypes RoleType { get; set; } = JwtClaimModelTypes.Role;
    }

    public class AuthenticationConfig
    {
        public bool IsPersistent { get; set; }
        public bool AllowRefresh { get; set; }
        public DateTimeOffset ExpiresUtc { get; set; }
        public string RedirectUri { get; set; }
    }
}