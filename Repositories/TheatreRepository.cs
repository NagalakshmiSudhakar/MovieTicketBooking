using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class TheatreRepository : ITheatreRepository
    {
        private readonly ApplicationDbContext _context;

        public TheatreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new theatre
        public async Task<Theatre> AddAsync(Theatre theatre)
        {
            await _context.Theatres.AddAsync(theatre);
            await _context.SaveChangesAsync();

            return theatre;
        }

        // Get all theatres
        public async Task<List<Theatre>> GetAllAsync()
        {
            return await _context.Theatres.ToListAsync();
        }

        // Get theatre by ID
        public async Task<Theatre?> GetByIdAsync(int theatreId)
        {
            return await _context.Theatres
                .FirstOrDefaultAsync(t => t.TheatreId == theatreId);
        }

        // Update theatre
        public async Task UpdateAsync(Theatre theatre)
        {
            _context.Theatres.Update(theatre);
            await _context.SaveChangesAsync();
        }

        // Delete theatre
        public async Task DeleteAsync(Theatre theatre)
        {
            _context.Theatres.Remove(theatre);
            await _context.SaveChangesAsync();
        }
    }
}