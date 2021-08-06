using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using Serilog.Context;

using KingdomApi.Services;

namespace KingdomApi.Middleware
{
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("CorrelationId", TraceIdService.GetTraceId(context)))
            {
                return _next.Invoke(context);
            }
        }
    }
}
