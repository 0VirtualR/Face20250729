
#region 1.创建WebApplication构建器
var builder = WebApplication.CreateBuilder(args);
//创建并初始化一个WebApplicationBuilder实例
//args参数包含从命令行传入的参数
//这会自动加载appsettings.json、环境变量等配置
#endregion

#region 2.添加服务到容器
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();//添加API Explorer服务
//为Swagger等工具提供API元数据

builder.Services.AddSwaggerGen();//添加Swagger生成器服务
//用于自动生成API文档
#endregion

//3.构建应用程序
var app = builder.Build();      //使用之前配置的服务构建WebApplication实例

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


app.UseAuthorization();//添加授权中间件        启用基于角色的授权功能

app.MapControllers();//映射属性路由控制器        将控制器中的路由配置应用到应用程序

app.Run();
