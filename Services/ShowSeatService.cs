
using MovieTicketBooking.DTOs.ShowSeats;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ShowSeatService : IShowSeatService
    {
        private readonly IShowSeatRepository _showSeatRepository;

        public ShowSeatService(IShowSeatRepository showSeatRepository)
        {
            _showSeatRepository = showSeatRepository;
        }

        public async Task<List<ShowSeatResponse>> GetSeatsByShowIdAsync(int showId)
        {
            var showExists = await _showSeatRepository.ShowExistsAsync(showId);

            if (!showExists)
                throw new KeyNotFoundException("Show not found.");

            var showSeats = await _showSeatRepository.GetByShowIdAsync(showId);

            return showSeats.Select(ss => new ShowSeatResponse
            {
                SeatId = ss.SeatId,
                RowName = ss.Seat!.RowName,
                SeatNumber = ss.Seat.SeatNumber,
                SeatLabel = $"{ss.Seat.RowName}{ss.Seat.SeatNumber}",
                SeatType = ss.Seat.SeatType,
                Price = ss.Show!.TicketPrice,
                Status = ss.Status
            }).ToList();
        }

        public async Task<List<ShowSeatResponse>> GetAvailableSeatsAsync(int showId)
        {
            var seats = await GetSeatsByShowIdAsync(showId);

            return seats
                .Where(s => s.Status == "Available")
                .ToList();
        }

        public async Task<List<ShowSeatResponse>> GetBookedSeatsAsync(int showId)
        {
            var seats = await GetSeatsByShowIdAsync(showId);

            return seats
                .Where(s => s.Status == "Booked")
                .ToList();
        }
    }
}
