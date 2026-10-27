using TaskManagerAPI.DTOs.Auth;

namespace TaskManagerAPI.Services
{
    public interface  IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
