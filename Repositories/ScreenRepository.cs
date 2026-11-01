using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
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
        public async Task<Screen> AddAsync(Screen screen)
        {
            await _context.Screens.AddAsync(screen);
            await _context.SaveChangesAsync();

            return screen;
        }

        // Get all screens
        public async Task<List<Screen>> GetAllAsync()
        {
            return await _context.Screens.ToListAsync();
        }

        // Get screen by ID
        public async Task<Screen?> GetByIdAsync(int screenId)
        {
            return await _context.Screens
                .FirstOrDefaultAsync(s => s.ScreenId == screenId);
        }

        // Get screens by Theatre ID
        public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
        {
            return await _context.Screens
                .Where(s => s.TheatreId == theatreId)
                .ToListAsync();
        }

        // Update screen
        public async Task UpdateAsync(Screen screen)
        {
            _context.Screens.Update(screen);
            await _context.SaveChangesAsync();
        }

        // Delete screen
        public async Task DeleteAsync(Screen screen)
        {
            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();
        }
    }
}