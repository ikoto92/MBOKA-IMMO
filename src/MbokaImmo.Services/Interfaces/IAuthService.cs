using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(AuthResponseDto Response, string RefreshToken)> RegisterAsync(RegisterRequestDto dto);
        Task<(AuthResponseDto Response, string RefreshToken)> LoginAsync(LoginRequestDto dto);
        Task<(AuthResponseDto Response, string RefreshToken)> RefreshAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
