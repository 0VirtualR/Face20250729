using AutoMapper;
using FaceAPI.Interface;
using FaceAPI.Model.SugarEntity;
using MyToDo.Shared.Contact;
using Shared;
using SqlSugar;

namespace FaceAPI.Service
{
    public class FaceService : IFaceService
    {
        private readonly ISqlSugarClient client;
        private readonly IMapper mapper;

        public FaceService(ISqlSugarClient client,IMapper mapper)
        {
            this.client = client;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(FaceDto faceDto)
        {
            var dbface = mapper.Map<T_FACE>(faceDto);
           var res=await client.Storageable(dbface).ExecuteCommandAsync();
           
               if(res > 0)
                {
                    return new ApiResponse(true, dbface);
                }
            
            return new ApiResponse("插入失败");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var result = await client.Deleteable<T_FACE>().Where(x => x.Id.Equals(id)).ExecuteCommandAsync();
           
                if (result > 0)
                {
                    return new ApiResponse(true, "");
                }
            
            return new ApiResponse("删除失败");
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter query)
        {
            try
            {
                int totalCount = 0;
                var items = await client.Queryable<T_FACE>().WhereIF(!string.IsNullOrWhiteSpace(query.Search), x => (x.Name.Contains(query.Search) || x.WorkName.Contains(query.Search))).ToPageListAsync(query.PageIndex, query.PageSize, totalCount);

                var pagelist = new PagedList<T_FACE>(items, query.PageIndex, query.PageSize ,1);
                //var res = await client.Queryable<T_FACE>().ToListAsync();
                return new ApiResponse(true, pagelist);
            }
           catch(Exception ex)
            {
                return new ApiResponse("获取所有数据失败");

            }


        }

      

        public async Task<ApiResponse> GetAsync(int id)
        {
            var res = await client.Queryable<T_FACE>().FirstAsync(x=>x.Id.Equals(id));
            if (res != null)
            {
                return new ApiResponse(true, res);
            }
            return new ApiResponse("获取单个数据失败");
        }

        public async Task<ApiResponse> UpdateAsync(FaceDto faceDto)
        {
            var dbface = mapper.Map<T_FACE>(faceDto);
            var res = await client.Storageable(dbface).ExecuteCommandAsync();
            if (res > 0)
            {
                return new ApiResponse(true, res);
            }
            return new ApiResponse("插入数据失败");
        }
    }
}
