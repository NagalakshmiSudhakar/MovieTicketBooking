using MovieTicketBooking.DTOs.Movies;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IMovieService
    {
        Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request);

        Task<List<MovieResponse>> GetAllMoviesAsync();

        Task<MovieResponse?> GetMovieByIdAsync(int movieId);

        Task<MovieResponse?> UpdateMovieAsync(int movieId, UpdateMovieRequest request);

        Task<bool> DeleteMovieAsync(int movieId);

        Task<List<MovieResponse>> SearchMoviesByTitleAsync(string title);
    }
}