namespace Kharazmi.AspNetMvc.Core.Managers.UrlManager
{
    public class AuthorizeRequest
    {
        public string ClientId { get; set; }
        public string ResponseType { get; set; }
        public string Scope { get; set; } = null;
        public string RedirectUri { get; set; } = null;
        public string State { get; set; } = null;
        public string Nonce { get; set; } = null;
        public string LoginHint { get; set; } = null;
        public string AcrValues { get; set; } = null;
        public string Prompt { get; set; } = null;
        public string ResponseMode { get; set; } = null;
        public string CodeChallenge { get; set; } = null;
        public string CodeChallengeMethod { get; set; } = null;
        public string Display { get; set; } = null;
        public int? MaxAge { get; set; } = null;
        public string UiLocales { get; set; } = null;
        public string IdTokenHint { get; set; } = null;
        public object Extra { get; set; } = null;
    }
}