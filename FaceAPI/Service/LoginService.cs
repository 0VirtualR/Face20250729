using AutoMapper;
using FaceAPI.Extensions;
using FaceAPI.Interface;
using FaceAPI.Model.SugarEntity;
using Shared;
using SqlSugar;

namespace FaceAPI.Service
{
    public class LoginService : ILoginService
    {
        private readonly ISqlSugarClient sqlSugarClient;
        private readonly IMapper mapper;

        public  LoginService(ISqlSugarClient sqlSugarClient,IMapper mapper)
        {
            this.sqlSugarClient = sqlSugarClient;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> LoginAsync(string UserName, string Password)
        {
            //Password=StringExtension.GetMD5(Password);
            //sqlSugarClient.Queryable<>
          var user=  await sqlSugarClient.Queryable<T_USER>().FirstAsync(user => user.Username.Equals(UserName) && user.Password.Equals(Password));
            if(user == null)
            {
                return new ApiResponse("登录失败");
            }
            else
            {
                return new ApiResponse(true, user);
            }
        }

        public async Task<ApiResponse> RegisterAsync(UserDto userDto)
        {

            var u = mapper.Map<T_USER>(userDto);
           bool isExist=await sqlSugarClient.Queryable<T_USER>().AnyAsync(user=>user.Username.Equals(u.Username));
            if (isExist)
            {
                return new ApiResponse("注册的用户已经存在:"+u.Username);
            }
            u.CreateTime = DateTime.Now.ToString("D");
             await sqlSugarClient.Insertable(u).ExecuteCommandAsync();
            u = await sqlSugarClient.Queryable<T_USER>().Where(user => user.Username == u.Username).FirstAsync();
            return new ApiResponse(true, u);
        }
    }
}
