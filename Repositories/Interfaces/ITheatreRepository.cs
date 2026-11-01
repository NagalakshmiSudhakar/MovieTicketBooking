using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ITheatreRepository
    {
        Task<Theatre> AddAsync(Theatre theatre);

        Task<List<Theatre>> GetAllAsync();

        Task<Theatre?> GetByIdAsync(int theatreId);

        Task UpdateAsync(Theatre theatre);

        Task DeleteAsync(Theatre theatre);
    }
}