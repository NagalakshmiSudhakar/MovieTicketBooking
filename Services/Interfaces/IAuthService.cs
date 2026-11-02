using MovieTicketBooking.DTOs.Authentication;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequest request);

        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}