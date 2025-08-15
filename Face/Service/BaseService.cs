using Face.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Face.Interface;
using Shared;
using Example;
using Face.Models;
using MyToDo.Shared.Contact;
using System.Net.Http;

namespace Face.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly string serviceName;
        private readonly IApiService apiService;

        public BaseService(string serviceName,IApiService apiService)
        {
            this.serviceName = serviceName;
            this.apiService = apiService;
        }
        public async Task<ApiResponse<T>> AddAsync(T entity)
        {
           BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = HttpMethod.Post;
            baseRequest.Rounte = $"api/{serviceName}/Add";
            baseRequest.Parameter = entity;
            return await apiService.SendAsync<T>(baseRequest);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
           BaseRequest baseRequest=new BaseRequest();
            baseRequest.Method = HttpMethod.Delete;
            baseRequest.Rounte=$"api/{serviceName}/Delete?id={ id} ";
          return  await apiService.SendAsync(baseRequest);
        }

        public async Task<ApiResponse<PagedList<T>>> GetAllAsync(QueryParameter query)
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = HttpMethod.Get;
            baseRequest.Rounte = $"api/{serviceName}/GetAll?&PageSize={query.PageSize}" + $"&PageIndex={query.PageIndex}" + $"&Search={query.Search}";
            return await apiService.SendAsync<PagedList<T>>(baseRequest);
        }

        public async Task<ApiResponse<T>> GetFirstOrDefaultAsync(int id)
        {
            BaseRequest baseRequest= new BaseRequest();
            baseRequest.Method = HttpMethod.Get;
            baseRequest.Rounte = $"api/{serviceName}/Get?id={id}";
            return await apiService.SendAsync<T>(baseRequest);
        }

        public async Task<ApiResponse<T>> UpdateAsync(T entity)
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = HttpMethod.Post;
            baseRequest.Rounte = $"api/{serviceName}/Update";
            baseRequest.Parameter=entity;
           return await apiService.SendAsync<T>(baseRequest);
        }
    }
}
