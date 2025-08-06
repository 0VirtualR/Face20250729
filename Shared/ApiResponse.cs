using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ApiResponse
    {
        public ApiResponse(bool status, object result)
        {

            this.Status = status;
            this.Result = result;
        }
        public ApiResponse(string msg, bool status = false)
        {
            this.Msg = msg;
            this.Status = status;
        }
        public bool Status { get; set; }
        public string Msg { get; set; }
        public object Result { get; set; }
    }
}
