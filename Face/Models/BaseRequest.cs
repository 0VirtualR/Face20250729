using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Face.Models
{
    public class BaseRequest
    {
        public HttpMethod Method{get;set;}
        public string Rounte { get; set; }
        public string ContentType { get; set; } = "application/json";
        public object Parameter { get; set; }
        //public Dictionary<string,string> Headers { get; set; }
    }
}
