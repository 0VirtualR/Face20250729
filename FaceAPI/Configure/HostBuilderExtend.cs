using Autofac;
using Autofac.Extensions.DependencyInjection;
using FaceAPI.Model.Other;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FaceAPI.Configure
{
    public static class HostBuilderExtend
    {
        public static void  Register(this WebApplicationBuilder builder)
        {
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(Sbuilder =>
            {
                #region`注册sqlsugar

                Sbuilder.Register<ISqlSugarClient>(context =>
                {
                    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
                    {
                        ConnectionString = builder.Configuration.GetConnectionString("ToDoConnection"),
                        DbType = DbType.Oracle,
                        IsAutoCloseConnection = true,

                    });
                    //支持sql语句的输出，方便排查问题
                    db.Aop.OnLogExecuted = (sql, par) =>
                    {
                        Console.WriteLine("\r\n");
                        Console.WriteLine($"{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}，Sql语句：{sql}");
                        Console.WriteLine("=================================================================================");
                    };
                    return db;
                });
                Sbuilder.RegisterType<DatabaseMigrationService>().SingleInstance();
                
                #endregion
                Sbuilder.RegisterModule(new AutofacModuleRegister());
            });

            //AddAuthentication  
            //第一步注册JWT
            builder.Services.Configure<JWTTokenOptions>(builder.Configuration.GetSection("JWTTokenOptions"));
            //第二步添加鉴权逻辑
            JWTTokenOptions tokenOptions = new JWTTokenOptions();
            builder.Configuration.Bind("JWTTokenOptions", tokenOptions);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
                };
            });
            //添加跨域策略
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
            });

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
       
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

             
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

        }
    }
}
/* 告诉 ASP.NET Core 使用 Autofac 作为依赖注入（DI）容器，替代默认的 MSDI（Microsoft.Extensions.DependencyInjection）。

为什么用 Autofac？

Autofac 提供更高级的 DI 功能，如：

属性注入（Property Injection）

更灵活的模块化注册（RegisterModule）

生命周期管理更精细（如 InstancePerLifetimeScope）*/
/*(2) builder.Host.ConfigureContainer<ContainerBuilder>()
作用：在 Autofac 容器中注册服务（类似于 builder.Services.AddXXX，但使用 Autofac 的语法）。

这里主要注册了两类服务：

SqlSugar ORM 数据库客户端（ISqlSugarClient）

自定义接口和实现类（通过 AutofacModuleRegister）*/
/*      X - Pagination 的典型场景
假设你的API返回分页数据：

csharp
// 后端Controller
[HttpGet("users")]
public IActionResult GetUsers(int page = 1, int pageSize = 10)
  {
      var users = _userService.GetPagedUsers(page, pageSize);
      var paginationMetadata = new
      {
          TotalCount = users.TotalCount,
          PageSize = users.PageSize,
          CurrentPage = users.CurrentPage,
          TotalPages = users.TotalPages
      };
      Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
      return Ok(users.Items);
  }
  前端需要读取分页信息：

javascript
fetch('/api/users')
.then(response => {
      const paginationHeader = response.headers.get('X-Pagination');
      const paginationData = JSON.parse(paginationHeader);
      console.log(`当前第${ paginationData.CurrentPage}
      页，共${ paginationData.TotalPages}
      页`);
  });
如果没有 WithExposedHeaders("X-Pagination")，前端将无法通过 response.headers.get() 获取到这个Header！*/

/*   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 的作用 问题场景：当对象有循环引用时
public class User
{
public int Id { get; set; }
public List<Order> Orders { get; set; }
}

public class Order
{
public int Id { get; set; }
public User Owner { get; set; }  // 循环引用！
}
序列化时会抛出异常！因为 User.Orders[0].Owner.Orders[0].Owner...无限循环。*/
/*

             options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; 作用：          统一日期格式 DateFormatString
             问题场景：

             .NET默认日期格式："2023-01-01T00:00:00Z"（ISO 8601格式）

             但前端可能期望 "2023-01-01 08:00:00"*/