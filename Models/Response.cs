using System;
using System.Collections.Generic;

namespace KingdomApi.Models
{
    public class ResponseObject<T>
    {
        public Boolean Status { get; set; }
        public String Message { get; set; }
        public Response<T> Response { get; set; }
    }

    public class Response<T>
    {
        public UInt16 Page { get; set; }
        public UInt16 PerPage { get; set; }
        public UInt32 Total { get; set; }
        public List<T> Results { get; set; }
    }
}
