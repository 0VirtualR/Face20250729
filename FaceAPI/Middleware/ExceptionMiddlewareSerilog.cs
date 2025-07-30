using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace FaceAPI.Middlewares
{
    public class ExceptionMiddlewareSerilog
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddlewareSerilog> _logger;

        public ExceptionMiddlewareSerilog(RequestDelegate next, ILogger<ExceptionMiddlewareSerilog> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var realEx = ex.GetBaseException();

                // 关键修改：使用Serilog的结构化日志记录方式
                _logger.LogError(realEx,
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] 异常报告\n" +
                    "请求ID: {RequestId}\n" +
                    "路径: {Path}\n" +
                    "方法: {Method}\n" +
                    "类型: {ExceptionType}\n" +
                    "信息: {Message}\n" +
                    "堆栈跟踪:\n{StackTrace}\n" +
                    "================================================================================\n",
                    DateTime.Now,
                    context.TraceIdentifier,
                    context.Request.Path,
                    $"{realEx.TargetSite?.DeclaringType?.Name}.{realEx.TargetSite?.Name}",
                    realEx.GetType().FullName,
                    realEx.Message,
                    realEx.StackTrace ?? "无堆栈信息" // 显式处理null情况
                );



                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = realEx.Message,
                    RequestId = context.TraceIdentifier
                });
            }
        }
    }

}
