using Face.Interface;
using Face.Models;
using MyToDo.Shared.Contact;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Face.Service
{
    public class FaceService : IFaceService
    {
        private readonly IApiService apiService;
        private readonly string serviceName="Face";

        public FaceService(IApiService apiService)
        {
            this.apiService = apiService;
        }

 
        public async Task<ApiResponse<PagedList<FaceDto>>> GetAll(QueryParameter query)
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = HttpMethod.Get;
            baseRequest.Rounte = $"api/{serviceName}/GetAll?&PageSize={query.PageSize}"+$"&PageIndex={query.PageIndex}"+$"&Search={query.Search}";
            return await apiService.SendAsync<PagedList<FaceDto>>(baseRequest);
        }
    }
}
