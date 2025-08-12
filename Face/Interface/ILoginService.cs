using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Interface
{
    public interface ILoginService
    {
        public Task<ApiResponse> Login(string username, string password);
 
        public Task<ApiResponse> Register(UserDto userDto);
    }
}
