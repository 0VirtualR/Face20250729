using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FaceAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                // 记录日志
                LogException(ex, context);

                context.Response.StatusCode= 500;
                await context.Response.WriteAsJsonAsync(new { Error = ex.Message,
                    RequestId = context.TraceIdentifier
                });

            }
        }
        private void LogException(Exception ex, HttpContext context)
        {
            var logContent = new StringBuilder()
                .AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] 异常报告")
                .AppendLine($"请求ID: {context.TraceIdentifier}")
                .AppendLine($"路径: {context.Request.Path}")
                .AppendLine($"类型: {ex.GetType().FullName}")
                .AppendLine($"信息: {ex.Message}")
                .AppendLine("堆栈跟踪:")
                .AppendLine(ex.StackTrace)
                .AppendLine(new string('=', 80))
                .ToString();

            // 线程安全的日志写入
            Task.Run(() => SafeWriteLog(logContent));
        }
      
        private static  void SafeWriteLog(string content)
        {
            try
            {
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFile");
                Directory.CreateDirectory(logPath);

                var logFile = Path.Combine(logPath, $"Error_{DateTime.Now:yyyyMMdd}.log");
                File.AppendAllText(logFile, content);
            }
            catch (Exception logEx)
            {
                Console.Error.WriteLine($"⚠️ 日志记录失败: {logEx.Message}");
            }
        }
    }
}
