using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApi
{
    public class XHttpHeaderOverrideMiddleware
    {
        private readonly RequestDelegate _next;

        public XHttpHeaderOverrideMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var headerValue = httpContext.Request.Headers["X-HTTP-Method-Override"];
            var queryValue = httpContext.Request.Query["X-HTTP-Method-Override"];
            if (!string.IsNullOrEmpty(headerValue))
            {
                httpContext.Request.Method = headerValue;
            }
            else if (!string.IsNullOrEmpty(queryValue))
            {
                httpContext.Request.Method = queryValue;
            }
            return _next.Invoke(httpContext);
        }
    }
}