using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.HTTP
{
    public interface IHttpResponse
    {
        public string HttpVersion { get; set; }
        public int ResponseCode { get; set; } 
        public string ResponseMessage { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string? Content { get; set; }
    }
}
