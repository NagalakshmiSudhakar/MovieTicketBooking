using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a show
        public async Task<Show> AddAsync(Show show)
        {
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();

            return show;
        }

        // Get all shows with optional filters
        public async Task<List<Show>> GetAllAsync(
            DateTime? date,
            int? movieId,
            int? theatreId)
        {
            var query = _context.Shows.AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(s =>
                    s.StartTime.Date == date.Value.Date);
            }

            if (movieId.HasValue)
            {
                query = query.Where(s => s.MovieId == movieId.Value);
            }

            if (theatreId.HasValue)
            {
                query = query.Where(s => s.Screen != null &&
                    s.Screen.TheatreId == theatreId.Value);
            }

            return await query
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        // Get show by ID
        public async Task<Show?> GetByIdAsync(int showId)
        {
            return await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == showId);
        }

        // Get shows for a movie
        public async Task<List<Show>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Shows
                .Where(s => s.MovieId == movieId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        // Get shows for a theatre
        public async Task<List<Show>> GetByTheatreIdAsync(int theatreId)
        {
            return await _context.Shows
                .Where(s => s.Screen != null &&
                    s.Screen.TheatreId == theatreId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        // Search shows by date
        public async Task<List<Show>> GetByDateAsync(DateTime date)
        {
            return await _context.Shows
                .Where(s => s.StartTime.Date == date.Date)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        // Check overlapping shows on the same screen
        public async Task<bool> HasOverlappingShowAsync(
            int screenId,
            DateTime startTime,
            DateTime endTime,
            int? excludeShowId = null)
        {
            var query = _context.Shows.Where(s =>
                s.ScreenId == screenId &&
                s.StartTime < endTime &&
                s.EndTime > startTime);

            if (excludeShowId.HasValue)
            {
                query = query.Where(s =>
                    s.ShowId != excludeShowId.Value);
            }

            return await query.AnyAsync();
        }

        // Update show
        public async Task UpdateAsync(Show show)
        {
            _context.Shows.Update(show);
            await _context.SaveChangesAsync();
        }

        // Delete show
        public async Task DeleteAsync(Show show)
        {
            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();
        }
    }
}