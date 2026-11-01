using MovieTicketBooking.DTOs.Seats;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ISeatService
    {
        Task<SeatResponse> CreateSeatAsync(CreateSeatRequest request);

        Task<List<SeatResponse>> BulkCreateSeatsAsync(
            BulkCreateSeatsRequest request);

        Task<List<SeatResponse>> GetSeatsByScreenIdAsync(int screenId);

        Task<SeatResponse?> GetSeatByIdAsync(int seatId);

        Task<SeatResponse?> UpdateSeatAsync(
            int seatId,
            UpdateSeatRequest request);

        Task<bool> DeleteSeatAsync(int seatId);
    }
}