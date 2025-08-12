using FaceAPI.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace FaceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        private readonly IFaceService faceService;

       public FaceController(IFaceService faceService)
        {
            this.faceService = faceService;
        }
        [HttpGet]
       public Task<ApiResponse> Get(int id) =>faceService.GetAsync(id);
        [HttpGet]
        public Task<ApiResponse> GetAll([FromQuery] QueryParameter query) =>faceService.GetAllAsync(query);
        [HttpPost]
        public Task<ApiResponse> Add(FaceDto faceDto)=>faceService.AddAsync(faceDto);
        [HttpPost]
        public Task<ApiResponse> Update(FaceDto faceDto)=>faceService.UpdateAsync(faceDto);
        [HttpDelete]
        public Task<ApiResponse> Delete(int id) =>faceService.DeleteAsync(id);
    }
}
