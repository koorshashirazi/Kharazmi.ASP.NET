using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Kharazmi.AspNetMvc.Core.Filters
{
    /// <summary>
    ///     Use Razor in JavaScript and CSS files
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ContentTypeFilterAttribute : ActionFilterAttribute
    {
        private readonly string _contentType;
        private readonly string _tag;

        public ContentTypeFilterAttribute(string contentType, string tag)
        {
            _contentType = contentType;
            _tag = tag;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Filter = new StripEnclosingTagsFilter(response.Filter, _tag);
            response.ContentType = _contentType;
        }

        private class StripEnclosingTagsFilter : MemoryStream
        {
            private static Regex _leadingOpeningScriptTag;
            private static Regex _trailingClosingScriptTag;

            //private static string Tag;

            private readonly StringBuilder _output;
            private readonly Stream _responseStream;

            /*static StripEnclosingTagsFilter()
            {
                LeadingOpeningScriptTag = new Regex(string.Format(@"^\s*<{0}[^>]*>", Tag), RegexOptions.Compiled);
                TrailingClosingScriptTag = new Regex(string.Format(@"</{0}>\s*$", Tag), RegexOptions.Compiled);
            }*/

            public StripEnclosingTagsFilter(Stream responseStream, string tag)
            {
                _leadingOpeningScriptTag = new Regex(string.Format(@"^\s*<{0}[^>]*>", tag), RegexOptions.Compiled);
                _trailingClosingScriptTag = new Regex(string.Format(@"</{0}>\s*$", tag), RegexOptions.Compiled);

                _responseStream = responseStream;
                _output = new StringBuilder();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                var response = GetStringResponse(buffer, offset, count);
                _output.Append(response);
            }

            public override void Flush()
            {
                var response = _output.ToString();

                if (_leadingOpeningScriptTag.IsMatch(response) && _trailingClosingScriptTag.IsMatch(response))
                {
                    response = _leadingOpeningScriptTag.Replace(response, string.Empty);
                    response = _trailingClosingScriptTag.Replace(response, string.Empty);
                }

                WriteStringResponse(response);
                _output.Clear();
            }

            private static string GetStringResponse(byte[] buffer, int offset, int count)
            {
                var responseData = new byte[count];
                Buffer.BlockCopy(buffer, offset, responseData, 0, count);

                return Encoding.Default.GetString(responseData);
            }

            private void WriteStringResponse(string response)
            {
                var outdata = Encoding.Default.GetBytes(response);
                _responseStream.Write(outdata, 0, outdata.GetLength(0));
            }
        }
    }
}