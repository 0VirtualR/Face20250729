using Face.Interface;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Face.Models;
using System.Net.Http;
using System.Windows.Controls;

namespace Face.Service
{
    public class LoginService : ILoginService
    {
        private readonly IApiService apiService;
        private readonly string  serviceName="Login";

        public LoginService(IApiService apiService )
        {
            this.apiService = apiService;
        }
        public async Task<ApiResponse> Login(string username, string password)
        {
            BaseRequest request = new BaseRequest();
            request.Method = HttpMethod.Post;
            request.Rounte = $"api/{serviceName}/Login";
            //request.Headers = new Dictionary<string, string>()
            //{
            //    { "page","12" }
            //};
            request.Parameter = new UserDto()
            {
                Username = username,
                Password = password
            };
         return   await apiService.SendAsync( request );

        }

        public Task<ApiResponse> Register(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
