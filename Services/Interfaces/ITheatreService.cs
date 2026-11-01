using MovieTicketBooking.DTOs.Theatres;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ITheatreService
    {
        Task<TheatreResponse> CreateTheatreAsync(
            CreateTheatreRequest request);

        Task<List<TheatreResponse>> GetAllTheatresAsync();

        Task<TheatreResponse?> GetTheatreByIdAsync(int theatreId);

        Task<TheatreResponse?> UpdateTheatreAsync(
            int theatreId,
            UpdateTheatreRequest request);

        Task<bool> DeleteTheatreAsync(int theatreId);
    }
}