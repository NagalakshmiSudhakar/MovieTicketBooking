
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IShowSeatRepository
    {
        Task<List<ShowSeat>> GetByShowIdAsync(int showId);

        Task<bool> ShowExistsAsync(int showId);
    }
}
