using Face.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Interface
{
    public interface IApiService
    {
     
      public  Task<ApiResponse> SendAsync(BaseRequest request);
      public  Task<ApiResponse<T>> SendAsync<T>(BaseRequest request);
    }
}
