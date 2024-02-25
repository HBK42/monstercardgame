using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.HTTP
{
    public interface IHttpRequest
    {
        public string? Content { get; set; }
    }
}
