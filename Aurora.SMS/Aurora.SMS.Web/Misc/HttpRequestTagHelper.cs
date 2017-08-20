using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Aurora.SMS.Web
{

    /// <summary>
    /// Accessing the Request object inside a Tag Helper in ASP.NET Core
    /// </summary>
    /// <remarks>http://www.jerriepelser.com/blog/accessing-request-object-inside-tag-helper-aspnet-core/</remarks>
    public class HttpRequestTagHelper : TagHelper
    {
        protected HttpRequest Request => ViewContext.HttpContext.Request;
        protected HttpResponse Response => ViewContext.HttpContext.Response;

        [ViewContext]
        public ViewContext ViewContext { get; set; }
    }
}
