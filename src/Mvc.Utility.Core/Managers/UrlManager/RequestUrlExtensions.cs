using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using Mvc.Utility.Core.Constraints;
using Mvc.Utility.Core.Extensions;

namespace Mvc.Utility.Core.Managers.UrlManager
{
    /// <summary>
    ///     Extensions for RequestUrl
    /// </summary>
    public static class RequestUrlExtensions
    {
        /// <summary>
        ///     Creates an authorize URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="values">The values (either using a string Dictionary or an object's properties).</param>
        /// <returns></returns>
        public static string Create(this RequestUrl request, object values)
        {
            return request.Create(values.ToDictionary());
        }

        /// <summary>
        ///     Creates an authorize URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="authorizeRequestUrl"></param>
        /// <returns></returns>
        public static string CreateAuthorizeUrl(this RequestUrl request, AuthorizeRequest authorizeRequestUrl)
        {
            var values = new Dictionary<string, string>
            {
                {OidcConstants.AuthorizeRequest.ClientId, authorizeRequestUrl.ClientId},
                {OidcConstants.AuthorizeRequest.ResponseType, authorizeRequestUrl.ResponseType}
            };

            values.AddOptional(OidcConstants.AuthorizeRequest.Scope, authorizeRequestUrl.Scope);
            values.AddOptional(OidcConstants.AuthorizeRequest.RedirectUri, authorizeRequestUrl.RedirectUri);
            values.AddOptional(OidcConstants.AuthorizeRequest.State, authorizeRequestUrl.State);
            values.AddOptional(OidcConstants.AuthorizeRequest.Nonce, authorizeRequestUrl.Nonce);
            values.AddOptional(OidcConstants.AuthorizeRequest.LoginHint, authorizeRequestUrl.LoginHint);
            values.AddOptional(OidcConstants.AuthorizeRequest.AcrValues, authorizeRequestUrl.AcrValues);
            values.AddOptional(OidcConstants.AuthorizeRequest.Prompt, authorizeRequestUrl.Prompt);
            values.AddOptional(OidcConstants.AuthorizeRequest.ResponseMode, authorizeRequestUrl.ResponseMode);
            values.AddOptional(OidcConstants.AuthorizeRequest.CodeChallenge, authorizeRequestUrl.CodeChallenge);
            values.AddOptional(OidcConstants.AuthorizeRequest.CodeChallengeMethod,
                authorizeRequestUrl.CodeChallengeMethod);
            values.AddOptional(OidcConstants.AuthorizeRequest.Display, authorizeRequestUrl.Display);
            values.AddOptional(OidcConstants.AuthorizeRequest.MaxAge, authorizeRequestUrl.MaxAge?.ToString());
            values.AddOptional(OidcConstants.AuthorizeRequest.UiLocales, authorizeRequestUrl.UiLocales);
            values.AddOptional(OidcConstants.AuthorizeRequest.IdTokenHint, authorizeRequestUrl.IdTokenHint);

            return request.Create(values.Merge(authorizeRequestUrl.Extra.ToDictionary()));
        }

        /// <summary>
        ///     Creates a end_session URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="idTokenHint">The id_token hint.</param>
        /// <param name="postLogoutRedirectUri">The post logout redirect URI.</param>
        /// <param name="state">The state.</param>
        /// <param name="extra">The extra parameters.</param>
        /// <returns></returns>
        public static string CreateEndSessionUrl(this RequestUrl request,
            string idTokenHint = null,
            string postLogoutRedirectUri = null,
            string state = null,
            object extra = null)
        {
            var values = new Dictionary<string, string>();

            values.AddOptional(OidcConstants.EndSessionRequest.IdTokenHint, idTokenHint);
            values.AddOptional(OidcConstants.EndSessionRequest.PostLogoutRedirectUri, postLogoutRedirectUri);
            values.AddOptional(OidcConstants.EndSessionRequest.State, state);

            return request.Create(values.Merge(extra.ToDictionary()));
        }

        /// <summary>
        ///     Append the given query key and value to the URI.
        /// </summary>
        /// <param name="uri">The base URI.</param>
        /// <param name="name">The name of the query key.</param>
        /// <param name="value">The query value.</param>
        /// <returns>The combined result.</returns>
        public static string AddQueryString(this string uri, string name, string value)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            if (name == null) throw new ArgumentNullException(nameof(name));

            if (value == null) throw new ArgumentNullException(nameof(value));

            return AddQueryString(uri, new[] {new KeyValuePair<string, string>(name, value)});
        }

        /// <summary>
        ///     Append the given query keys and values to the uri.
        /// </summary>
        /// <param name="uri">The base uri.</param>
        /// <param name="queryString">A collection of name value query pairs to append.</param>
        /// <returns>The combined result.</returns>
        public static string AddQueryString(this string uri, IDictionary<string, string> queryString)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            if (queryString == null) throw new ArgumentNullException(nameof(queryString));

            return AddQueryString(uri, (IEnumerable<KeyValuePair<string, string>>) queryString);
        }

        private static string AddQueryString(string uri, IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            if (queryString == null) throw new ArgumentNullException(nameof(queryString));

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";
            // If there is an anchor, then the query string must be inserted before its first occurance.
            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);
            foreach (var parameter in queryString)
            {
                if (parameter.Value == null) continue;

                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }

        public static void AddOptional(this IDictionary<string, string> parameters, string key, string value)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (!value.IsPresent()) return;

            if (parameters.ContainsKey(key)) throw new InvalidOperationException($"Duplicate parameter: {key}");

            parameters.Add(key, value);
        }

        public static void AddRequired(this IDictionary<string, string> parameters, string key, string value,
            bool allowEmpty = false)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (value.IsPresent())
            {
                if (parameters.ContainsKey(key)) throw new InvalidOperationException($"Duplicate parameter: {key}");

                parameters.Add(key, value);
            }
            else
            {
                if (allowEmpty)
                    parameters.Add(key, "");
                else
                    throw new ArgumentException(ShareResources.ParameterRequired, key);
            }
        }
    }
}