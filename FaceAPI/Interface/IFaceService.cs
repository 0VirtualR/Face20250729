using Shared;

namespace FaceAPI.Interface
{
    public interface IFaceService
    {
        Task<ApiResponse> GetAsync(int id);
        Task<ApiResponse> GetAllAsync(QueryParameter query);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> UpdateAsync(FaceDto faceDto);
        Task<ApiResponse> AddAsync(FaceDto faceDto);
    }
}
