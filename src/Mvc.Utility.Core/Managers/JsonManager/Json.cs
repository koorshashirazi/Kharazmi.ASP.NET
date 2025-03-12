using System;
using System.Web.Mvc;
using Mvc.Utility.Core.Constraints;
using Mvc.Utility.Core.Helpers;
using Newtonsoft.Json;

namespace Mvc.Utility.Core.Managers.JsonManager
{
    public class Json : JsonResult
    {
        public Json()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                Formatting = Formatting.Indented
            };
        }

        public JsonSerializerSettings Settings { get; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw ExceptionHelper.ThrowException<ArgumentNullException>(string.Empty, nameof(context));

            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw ExceptionHelper.ThrowException<InvalidOperationException>(ShareResources.ErrorAllowGetJosn);

            response.ContentType = string.IsNullOrEmpty(ContentType) ? Constraint.JSON_CONTENT_TYPE : ContentType;
            if (ContentEncoding != null) response.ContentEncoding = ContentEncoding;

            if (Data == null) return;

            var json = JsonConvert.SerializeObject(Data, Settings);
            response.Write(json);
        }
    }
}