
using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request);

        Task<List<BookingResponse>> GetAllBookingsAsync(
            int? showId,
            string? status);

        Task<BookingResponse?> GetBookingByIdAsync(int bookingId);

        Task<BookingResponse?> CancelBookingAsync(int bookingId);
    }
}
