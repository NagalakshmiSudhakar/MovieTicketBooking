using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<Seat> AddAsync(Seat seat);

        Task<List<Seat>> AddRangeAsync(List<Seat> seats);

        Task<List<Seat>> GetByScreenIdAsync(int screenId);

        Task<Seat?> GetByIdAsync(int seatId);

        Task UpdateAsync(Seat seat);

        Task DeleteAsync(Seat seat);

        Task<bool> ExistsAsync(
            int screenId,
            string rowName,
            int seatNumber);
    }
}