using System.Diagnostics;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace KingdomApi.Services
{
    public class TraceIdService
    {

        public static string GetTraceId(HttpContext context)
        {
            context.Request.Headers.TryGetValue("Cko-Correlation-Id", out StringValues correlationId);
            return correlationId.FirstOrDefault() ?? Activity.Current?.Id ?? context.TraceIdentifier;
        }
    }
}
