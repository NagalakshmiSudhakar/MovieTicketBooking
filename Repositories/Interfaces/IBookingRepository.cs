
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> AddAsync(Booking booking);

        Task<List<Booking>> GetAllAsync(int? showId, string? status);
        Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId);

        Task<Booking?> GetByIdAsync(int bookingId);

        Task AddBookingSeatsAsync(List<BookingSeat> bookingSeats);
        Task<List<Booking>> GetByUserIdAsync(string userId);
        Task UpdateAsync(Booking booking);
    }
}
