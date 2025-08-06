using Shared;

namespace FaceAPI.Interface
{
    public interface ILoginService
    {
        Task<ApiResponse> LoginAsync(string UserName,string Password);
        Task<ApiResponse> RegisterAsync(UserDto userDto);
    }
}
