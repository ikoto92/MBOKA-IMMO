using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
    }
}
