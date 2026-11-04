
namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IScreenRepository
    {
        Task<MovieTicketBooking.Models.Screen> AddAsync(
            MovieTicketBooking.Models.Screen screen);

        Task<List<MovieTicketBooking.Models.Screen>> GetAllAsync();

        Task<MovieTicketBooking.Models.Screen?> GetByIdAsync(int screenId);

        Task<List<MovieTicketBooking.Models.Screen>> GetByTheatreIdAsync(int theatreId);

        Task UpdateAsync(MovieTicketBooking.Models.Screen screen);

        Task DeleteAsync(MovieTicketBooking.Models.Screen screen);
    }
}
