
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowRepository _showRepository;
        private readonly IShowSeatRepository _showSeatRepository;

        public BookingService(
            ApplicationDbContext context,
            IBookingRepository bookingRepository,
            IShowRepository showRepository,
            IShowSeatRepository showSeatRepository)
        {
            _context = context;
            _bookingRepository = bookingRepository;
            _showRepository = showRepository;
            _showSeatRepository = showSeatRepository;
        }

        public async Task<BookingResponse> CreateBookingAsync(
            CreateBookingRequest request)
        {
            if (request.SeatIds == null || request.SeatIds.Count == 0)
                throw new InvalidOperationException("Select at least one seat.");

            if (request.SeatIds.Any(id => id <= 0))
                throw new InvalidOperationException("Invalid seat ID.");

            if (request.SeatIds.Distinct().Count() != request.SeatIds.Count)
                throw new InvalidOperationException("Duplicate seat IDs are not allowed.");

            var show = await _showRepository.GetByIdAsync(request.ShowId);

            if (show == null)
                throw new KeyNotFoundException("Show not found.");

            var showSeats = await _showSeatRepository
                .GetByShowIdAsync(request.ShowId);

            var selectedSeats = showSeats
                .Where(ss => request.SeatIds.Contains(ss.SeatId))
                .ToList();

            if (selectedSeats.Count != request.SeatIds.Count)
                throw new InvalidOperationException(
                    "One or more seats do not belong to this show.");

            if (selectedSeats.Any(ss => ss.Status != "Available"))
                throw new InvalidOperationException(
                    "One or more selected seats are already booked.");

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = new Booking
                {
                    BookingReference = $"MOV{Guid.NewGuid():N}"[..15],
                    ShowId = request.ShowId,
                    CustomerName = request.CustomerName,
                    CustomerEmail = request.CustomerEmail,
                    CustomerPhone = request.CustomerPhone,
                    BookingDate = DateTime.UtcNow,
                    TotalAmount = show.TicketPrice * selectedSeats.Count,
                    Status = "Confirmed"
                };

                await _bookingRepository.AddAsync(booking);

                var bookingSeats = selectedSeats.Select(ss => new BookingSeat
                {
                    BookingId = booking.BookingId,
                    SeatId = ss.SeatId,
                    Price = show.TicketPrice
                }).ToList();

                await _bookingRepository.AddBookingSeatsAsync(bookingSeats);

                foreach (var seat in selectedSeats)
                {
                    seat.Status = "Booked";
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await BuildBookingResponseAsync(booking);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<BookingResponse>> GetAllBookingsAsync(
            int? showId,
            string? status)
        {
            var bookings = await _bookingRepository.GetAllAsync(showId, status);

            var responses = new List<BookingResponse>();

            foreach (var booking in bookings)
            {
                responses.Add(await BuildBookingResponseAsync(booking));
            }

            return responses;
        }

        public async Task<BookingResponse?> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                return null;

            return await BuildBookingResponseAsync(booking);
        }

        public async Task<BookingResponse?> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                return null;

            if (booking.Status != "Confirmed")
                throw new InvalidOperationException(
                    "Only confirmed bookings can be cancelled.");

            var bookingSeats = await _bookingRepository
                .GetBookingSeatsAsync(bookingId);

            var showSeats = await _showSeatRepository
                .GetByShowIdAsync(booking.ShowId);

            var seatIds = bookingSeats.Select(bs => bs.SeatId).ToHashSet();

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var showSeat in showSeats.Where(
                    ss => seatIds.Contains(ss.SeatId)))
                {
                    showSeat.Status = "Available";
                }

                booking.Status = "Cancelled";

                await _bookingRepository.UpdateAsync(booking);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await BuildBookingResponseAsync(booking);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<BookingResponse> BuildBookingResponseAsync(
            Booking booking)
        {
            var bookingSeats = await _bookingRepository
                .GetBookingSeatsAsync(booking.BookingId);

            var seatIds = bookingSeats.Select(bs => bs.SeatId).ToList();

            var seats = await _context.Seats
                .Where(s => seatIds.Contains(s.SeatId))
                .ToListAsync();

            return new BookingResponse
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReference,
                ShowId = booking.ShowId,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status,
                Seats = bookingSeats.Select(bs =>
                {
                    var seat = seats.First(s => s.SeatId == bs.SeatId);

                    return new BookingSeatResponse
                    {
                        SeatId = bs.SeatId,
                        SeatLabel = $"{seat.RowName}{seat.SeatNumber}",
                        Price = bs.Price
                    };
                }).ToList()
            };
        }
    }
}
