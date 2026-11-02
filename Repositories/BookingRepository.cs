
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Booking>> GetByUserIdAsync(string userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<List<Booking>> GetAllAsync(int? showId, string? status)
        {
            var query = _context.Bookings.AsQueryable();

            if (showId.HasValue)
            {
                query = query.Where(b => b.ShowId == showId.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(b => b.Status == status);
            }

            return await query
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task AddBookingSeatsAsync(List<BookingSeat> bookingSeats)
        {
            await _context.BookingSeats.AddRangeAsync(bookingSeats);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId)
        {
            return await _context.BookingSeats
                .Where(bs => bs.BookingId == bookingId)
                .ToListAsync();
        }
    }
}
