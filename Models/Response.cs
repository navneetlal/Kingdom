using System.Collections.Generic;

namespace KingdomApi.Models
{
    public class ResponseObject<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Response<T> Response { get; set; }
    }

    public class Response<T>
    {
        public ushort Page { get; set; }
        public ushort PerPage { get; set; }
        public uint Total { get; set; }
        public List<T> Results { get; set; }
    }
}
