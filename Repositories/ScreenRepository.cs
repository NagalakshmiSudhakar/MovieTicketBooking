
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly ApplicationDbContext _context;

        public ScreenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new screen
        public async Task<MovieTicketBooking.Models.Screen> AddAsync(
            MovieTicketBooking.Models.Screen screen)
        {
            await _context.Screens.AddAsync(screen);
            await _context.SaveChangesAsync();

            return screen;
        }

        // Get all screens
        public async Task<List<MovieTicketBooking.Models.Screen>> GetAllAsync()
        {
            return await _context.Screens.ToListAsync();
        }

        // Get screen by ID
        public async Task<MovieTicketBooking.Models.Screen?> GetByIdAsync(
            int screenId)
        {
            return await _context.Screens
                .FirstOrDefaultAsync(s => s.ScreenId == screenId);
        }

        // Get screens by Theatre ID
        public async Task<List<MovieTicketBooking.Models.Screen>> GetByTheatreIdAsync(
            int theatreId)
        {
            return await _context.Screens
                .Where(s => s.TheatreId == theatreId)
                .ToListAsync();
        }

        // Update screen
        public async Task UpdateAsync(
            MovieTicketBooking.Models.Screen screen)
        {
            _context.Screens.Update(screen);
            await _context.SaveChangesAsync();
        }

        // Delete screen
        public async Task DeleteAsync(
            MovieTicketBooking.Models.Screen screen)
        {
            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();
        }
    }
}
