using System;
using System.Linq;
using System.Net;
using System.Text;
using Mvc.Utility.Core.Managers.HtmlManager;

namespace Mvc.Utility.Core.Filters
{
    /// <summary>
    ///     TODO
    /// </summary>
    internal class HtmlParserFilterAttribute
    {
        public Action<string> ParseError { set; get; }

        public Func<HtmlNode, bool> ParserHtmlNode { set; get; }

        public void StartParsingHtml(Uri url)
        {
            using (var client = new WebClient {Encoding = Encoding.UTF8})
            {
                client.Headers.Add("user-agent",
                    "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                StartParsingHtml(client.DownloadString(url));
            }
        }

        public void StartParsingHtml(string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(htmlContent)) throw new ArgumentNullException(nameof(htmlContent));

            var doc = new HtmlDocument
            {
                OptionCheckSyntax = true,
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };
            doc.LoadHtml(htmlContent);

            if (doc.ParseErrors != null && doc.ParseErrors.Any())
                foreach (var error in doc.ParseErrors)
                    ParseError?.Invoke(error.Code + " - " + error.Reason);

            if (!doc.DocumentNode.HasChildNodes) return;

            HandleChildren(doc.DocumentNode.ChildNodes);
        }

        private void HandleChildren(HtmlNodeCollection nodes)
        {
            foreach (var itm in nodes)
                if (itm.Name.ToLower().Equals("html"))
                {
                    if (itm.Element("body") != null) HandleChildren(itm.Element("body").ChildNodes);
                }
                else
                {
                    HandleHtmlNode(itm);
                }
        }

        private void ParserChildNodes(HtmlNode content)
        {
            foreach (var item in content.ChildNodes) HandleHtmlNode(item);
        }

        private void HandleHtmlNode(HtmlNode htmNode)
        {
            switch (htmNode.Name.ToLower())
            {
                case "html":
                case "body":
                    HandleChildren(htmNode.ChildNodes);
                    break;

                default:
                    if (ParserHtmlNode == null) throw new ArgumentNullException(nameof(ParserHtmlNode));

                    if (ParserHtmlNode(htmNode)) ParserChildNodes(htmNode);

                    break;
            }
        }
    }
}