
#region 1.创建WebApplication构建器
using FaceAPI.Configure;
using FaceAPI.Middlewares;
using Serilog;
using Serilog.Events;
using static System.Net.WebRequestMethods;



/*.MinimumLevel.Information()                 // 全局最低日志级别
.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)  // 覆盖Microsoft命名空间的日志级别
.MinimumLevel.Override("System", LogEventLevel.Warning)     // 覆盖System命名空间的日志级别
作用说明
配置方法	作用	示例影响
.MinimumLevel.Information()	全局最低日志级别：
只记录 Information 及以上级别（Info, Warning, Error, Fatal）	Debug 日志会被忽略
.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)	覆盖 Microsoft. 开头的类库日志级别：
只记录 Warning 及以上级别	过滤掉 ASP.NET Core 框架的低级别日志（如路由匹配、中间件启动等）
.MinimumLevel.Override("System", LogEventLevel.Warning)	覆盖 System. 开头的类库日志级别：
只记录 Warning 及以上级别	过滤掉 .NET 底层系统组件的低级别日志
*/

Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning).
    MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Console().
    WriteTo.File("LogFile/webapi-.log", 
    rollingInterval: RollingInterval.Day,
    retainedFileCountLimit:7,
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
    ).
    CreateLogger();

try
{
    Log.Information("Starting web host...");
    var builder = WebApplication.CreateBuilder(args);
    //创建并初始化一个WebApplicationBuilder实例
    //args参数包含从命令行传入的参数
    //这会自动加载appsettings.json、环境变量等配置
    #endregion

    builder.Host.UseSerilog();      //使用Serilog进行日志打印

    #region 2.添加服务到容器
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();//添加API Explorer服务
                                               //为Swagger等工具提供API元数据

    builder.Services.AddSwaggerGen();//添加Swagger生成器服务
                                     //用于自动生成API文档
    #endregion

    builder.Register();


    //3.构建应用程序
    var app = builder.Build();      //使用之前配置的服务构建WebApplication实例

    app.UseMiddleware<ExceptionMiddlewareSerilog>();    //使用Serilog日志中间件

    // 4. 配置HTTP请求管道
    //if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        //添加Swagger中间件（即使注释掉了环境检查，也会始终启用）

        //UseSwagger()：生成Swagger JSON文档

        //UseSwaggerUI()：提供Swagger UI界面
    }

    app.UseHttpsRedirection();//启用HTTPS重定向中间件 
    /*作用：将所有HTTP请求重定向到HTTPS

    实际效果：

    如果用户访问 http://example.com/api/values

    自动重定向到 https://example.com/api/values

    重要性：安全最佳实践，特别是生产环境*/


    app.UseAuthentication();    //鉴权中间件
    app.UseAuthorization();//添加授权中间件        启用基于角色的授权功能

    app.UseCors("CorsPolicy");

    app.MapControllers();//映射属性路由控制器        将控制器中的路由配置应用到应用程序

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!"); // 捕获启动异常
}
finally
{
    Log.CloseAndFlush();
}
/*  Serilog的使用
 * 既然已经有 try-catch 捕获全局异常，为什么还需要 ExceptionMiddlewareSerilog？
两者的作用不同
机制  作用范围 捕获的异常   典型用途
try-catch（Main 方法）	程序启动阶段 CreateHostBuilder().Build().Run() 的异常
（如端口冲突、配置错误）	记录崩溃日志，优雅退出
ExceptionMiddlewareSerilog  HTTP 请求阶段   控制器、中间件等处理请求时的异常 返回 JSON 错误响应，记录请求上下文
为什么两者都需要？
Main 的 try-catch 无法捕获 HTTP 请求异常

它只捕获 Host 启动时的异常（如 app.Run() 之前的问题）。

一旦 app.Run() 执行，后续的 HTTP 请求异常由中间件处理。

ExceptionMiddlewareSerilog 提供更友好的 API 错误响应

直接返回 JSON 格式的错误信息（如 { "Error": "xxx", "RequestId": "123" }）。

记录请求上下文（如 URL、HTTP 方法、请求 ID）。

示例场景
Main 的 try-catch 捕获：

数据库连接失败、配置文件缺失、端口被占用。

ExceptionMiddlewareSerilog 捕获：

UserController.GetById(999) 抛出 NullReferenceException。*/