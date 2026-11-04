
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Exceptions;
using System.Security.Claims;

namespace MovieTicketBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowRepository _showRepository;
        private readonly IShowSeatRepository _showSeatRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingService(
             ApplicationDbContext context,
             IBookingRepository bookingRepository,
             IShowRepository showRepository,
             IShowSeatRepository showSeatRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _bookingRepository = bookingRepository;
            _showRepository = showRepository;
            _showSeatRepository = showSeatRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BookingResponse> CreateBookingAsync(
            CreateBookingRequest request)
        {
            var userId = _httpContextAccessor.HttpContext?
    .User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            if (request.SeatIds == null || request.SeatIds.Count == 0)
                throw new BadRequestException("Select at least one seat.");

            if (request.SeatIds.Any(id => id <= 0))
                throw new BadRequestException("Invalid seat ID.");

            if (request.SeatIds.Distinct().Count() != request.SeatIds.Count)
                throw new BadRequestException("Duplicate seat IDs are not allowed.");

            var show = await _showRepository.GetByIdAsync(request.ShowId);

            if (show == null)
                throw new NotFoundException("Show not found.");

            var showSeats = await _showSeatRepository
                .GetByShowIdAsync(request.ShowId);

            var selectedSeats = showSeats
                .Where(ss => request.SeatIds.Contains(ss.SeatId))
                .ToList();

            if (selectedSeats.Count != request.SeatIds.Count)
                throw new ConflictException(
                    "One or more seats do not belong to this show.");

            if (selectedSeats.Any(ss => ss.Status != "Available"))
                throw new ConflictException(
                    "One or more selected seats are already booked.");

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = new Booking
                {
                    BookingReference = $"MOV{Guid.NewGuid():N}"[..15],
                    ShowId = request.ShowId,
                    UserId = userId,
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
            List<Booking> bookings;

            var currentUser = _httpContextAccessor.HttpContext?.User;

            if (currentUser == null || !currentUser.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            if (currentUser.IsInRole("Admin"))
            {
                bookings = await _bookingRepository.GetAllAsync(showId, status);
            }
            else
            {
                var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedException("User ID not found.");
                }

                bookings = await _bookingRepository.GetByUserIdAsync(userId);

                if (showId.HasValue)
                {
                    bookings = bookings
                        .Where(b => b.ShowId == showId.Value)
                        .ToList();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    bookings = bookings
                        .Where(b => b.Status == status)
                        .ToList();
                }
            }

            var responses = new List<BookingResponse>();

            foreach (var booking in bookings)
            {
                responses.Add(await BuildBookingResponseAsync(booking));
            }

            return responses;
        }

        public async Task<BookingResponse?> GetBookingByIdAsync(int bookingId)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;

            if (currentUser == null || !currentUser.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                return null;

            // Admin can access every booking
            if (currentUser.IsInRole("Admin"))
            {
                return await BuildBookingResponseAsync(booking);
            }

            // Customer can access only their own booking
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException("User ID not found.");
            }

            if (booking.UserId != userId)
            {
                throw new NotFoundException("Booking not found.");
            }

            return await BuildBookingResponseAsync(booking);
        }
        

        public async Task<BookingResponse?> CancelBookingAsync(int bookingId)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;

            if (currentUser == null || !currentUser.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                return null;

            // Admin can cancel any booking
            if (!currentUser.IsInRole("Admin"))
            {
                var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedException("User ID not found.");
                }

                if (booking.UserId != userId)
                {
                    throw new NotFoundException("Booking not found.");
                }
            }

            if (booking.Status != "Confirmed")
                throw new ConflictException(
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
