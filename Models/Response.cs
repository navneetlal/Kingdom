using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using KingdomApi.Services;

namespace KingdomApi.Models
{
    public class ResponseObject<T>
    {
        public int Status { get; set; }
        public string Title { get; set; }
        public string TraceId { get; }
        public Response<T> Response { get; set; }

        public ResponseObject(HttpContext context)
        {
            TraceId = TraceIdService.GetTraceId(context);
        }
    }

    public class Response<T>
    {
        public ushort? Page { get; set; }
        public ushort? PerPage { get; set; }
        public uint? Total { get; set; }
        public List<T> Results { get; set; }
    }
}
