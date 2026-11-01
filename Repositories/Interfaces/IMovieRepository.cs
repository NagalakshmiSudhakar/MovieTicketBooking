
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie> AddAsync(Movie movie);

        Task<List<Movie>> GetAllAsync();

        Task<Movie?> GetByIdAsync(int movieId);

        Task UpdateAsync(Movie movie);

        Task DeleteAsync(Movie movie);

        Task<List<Movie>> SearchByTitleAsync(string title);
    }
}
