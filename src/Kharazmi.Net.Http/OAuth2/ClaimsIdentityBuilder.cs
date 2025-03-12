using Mvc.Utility.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using IdentityModel.Client;

namespace Kharazmi.Net.Http.OAuth2
{
    public static class ClaimsIdentityBuilder
    {
        public static ClaimsIdentity AddIdTokenClaim(this ClaimsIdentity claimsIdentity, TokenResponse tokenResponse)
        {
            if (string.IsNullOrWhiteSpace(tokenResponse.IdentityToken))
                throw ExceptionHelper.ThrowException<ArgumentNullException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                        nameof(Constraints.ID_TOKEN)), nameof(Constraints.ID_TOKEN));

            claimsIdentity.AddClaim(new Claim(Constraints.ID_TOKEN, tokenResponse.IdentityToken));
            return claimsIdentity;
        }

        public static ClaimsIdentity AddAccessTokenToClaims(this ClaimsIdentity claimsIdentity,
            TokenResponse tokenResponse)
        {
            if (!string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                claimsIdentity.AddClaim(
                    new Claim(Constraints.ACCESS_TOKEN, tokenResponse.AccessToken));

            return claimsIdentity;
        }

        public static ClaimsIdentity AddRefreshTokenToClaims(this ClaimsIdentity claimsIdentity,
            TokenResponse tokenResponse)
        {
            if (!string.IsNullOrWhiteSpace(tokenResponse.RefreshToken))
                claimsIdentity.AddClaim(new Claim(Constraints.REFRESH_TOKEN,
                    tokenResponse.RefreshToken));

            return claimsIdentity;
        }

        public static ClaimsIdentity AddExpiresAtToClaims(this ClaimsIdentity claimsIdentity,
            TokenResponse tokenResponse)
        {
            if (tokenResponse == null)
                throw ExceptionHelper.ThrowException<ArgumentNullException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                        nameof(tokenResponse)), nameof(tokenResponse));

            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            var claims = new List<Claim>
            {
                new Claim(Constraints.EXPIRES_AT,
                    expiresAt.ToString("o", CultureInfo.InvariantCulture))
            };
            claimsIdentity.AddClaims(claims);

            return claimsIdentity;
        }

        public static ClaimsIdentity AddUserInfoToClaims(this ClaimsIdentity claimsIdentity,
            TokenResponse tokenResponse)
        {
            if (tokenResponse == null)
                throw ExceptionHelper.ThrowException<ArgumentNullException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                        nameof(tokenResponse)), nameof(tokenResponse));

            if (string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                throw ExceptionHelper.ThrowException<ArgumentNullException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                        nameof(Constraints.ACCESS_TOKEN), nameof(Constraints.ACCESS_TOKEN)),
                    nameof(Constraints.ACCESS_TOKEN));

            var client = DiscoverySingleton.Instance.HttpClient(Constraints.BASE_ADDRESS);

            var disco = DiscoverySingleton.Instance.GetDiscoveryResponse(Constraints.BASE_ADDRESS);

            var userInfo = client.GetUserInfoAsync(new UserInfoRequest
            {
                Token = tokenResponse.AccessToken,
                Address = disco.UserInfoEndpoint
            }).GetAwaiter().GetResult();

            claimsIdentity.AddClaims(userInfo.Claims);
            return claimsIdentity;
        }

        public static ClaimsIdentity RemoveClaims(this ClaimsIdentity claimsIdentity, IEnumerable<string> claims)
        {
            if (claims == null)
                throw ExceptionHelper.ThrowException<ArgumentNullException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                        nameof(claims)), nameof(claims));

            var claimsToKeep =
                claimsIdentity.Claims.Where(x => claims.Contains(x.Type)).ToList();

            foreach (var item in claimsToKeep) claimsIdentity.RemoveClaim(item);

            return claimsIdentity;
        }

        public static ClaimsIdentity AddExtraClaims(this ClaimsIdentity claimsIdentity, IEnumerable<Claim> claims)
        {
            if (claims == null)
                throw ExceptionHelper.ThrowException<ArgumentNullException>(
                    ResourceHelper.ReadResourceValue(typeof(ShareResource), nameof(ArgumentNullException),
                        nameof(claims)), nameof(claims));

            claimsIdentity.AddClaims(claims);

            return claimsIdentity;
        }
    }
}