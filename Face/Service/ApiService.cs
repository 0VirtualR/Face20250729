using Face.Interface;
using Face.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Face.Service
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<ApiResponse> SendAsync(BaseRequest request)
        {
            var httpRequest = new HttpRequestMessage(request.Method, request.Rounte);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //if (request.Headers != null)
            //{
            //    foreach(var item in  request.Headers)
            //    {
            //        httpRequest.Headers.Add(item.Key, item.Value);
            //    }
            //}
            if(request.Parameter != null)
            {
                httpRequest.Content=new StringContent(JsonSerializer.Serialize(request.Parameter),Encoding.UTF8,request.ContentType);
            }
            var result=await httpClient.SendAsync(httpRequest);

            if(result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<object>(content);
                return new ApiResponse()
                {
                    Status = true,
                    Result = data
                };
            }
            else
            {
                var error=await result.Content.ReadAsStringAsync();
                return new ApiResponse()
                {
                    Status = false,
                    Msg = $"HTTP错误: {result.StatusCode} - {error}"
                };
            }
        }
        public async Task<ApiResponse<T>> SendAsync<T>(BaseRequest request)
        {
            //try
            {
                // 1. 创建HttpRequestMessage
                var httpRequest = new HttpRequestMessage(request.Method, request.Rounte);

                // 2. 添加请求头（默认+自定义）
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //if (request.Headers != null)
                //{
                //    foreach (var header in request.Headers)
                //    {
                //        httpRequest.Headers.Add(header.Key, header.Value);
                //    }
                //}

                // 3. 添加请求体（如果是Post/Put等）
                if (request.Parameter != null && (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put))
                {
                    httpRequest.Content = new StringContent(
                        JsonSerializer.Serialize(request.Parameter),
                        Encoding.UTF8,
                        "application/json"
                    );
                }

                // 4. 发送请求
                var response = await httpClient.SendAsync(httpRequest);

                // 5. 处理响应
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<T>(content);
                    return new ApiResponse<T> { Status = true, Result = data };
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<T>
                    {
                        Status = false,
                        Msg = $"HTTP错误: {response.StatusCode} - {error}"
                    };
                }
            }
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "API请求失败: {Url}", request.Url);
            //    return new ApiResponse<T> { IsSuccess = false, ErrorMessage = ex.Message };
            //}
        }
    }
}
