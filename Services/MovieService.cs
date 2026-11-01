using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // Create a new movie
        public async Task<MovieResponse> CreateMovieAsync(
            CreateMovieRequest request)
        {
            var movie = new Movie
            {
                Title = request.Title,
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                ReleaseDate = request.ReleaseDate,
                Language = request.Language,
                Genre = request.Genre
            };

            var createdMovie = await _movieRepository.AddAsync(movie);

            return MapToResponse(createdMovie);
        }

        // Get all movies
        public async Task<List<MovieResponse>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.GetAllAsync();

            return movies.Select(MapToResponse).ToList();
        }

        // Get movie by ID
        public async Task<MovieResponse?> GetMovieByIdAsync(int movieId)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);

            if (movie == null)
                return null;

            return MapToResponse(movie);
        }

        // Update movie
        public async Task<MovieResponse?> UpdateMovieAsync(
            int movieId, UpdateMovieRequest request)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);

            if (movie == null)
                return null;

            movie.Title = request.Title;
            movie.Description = request.Description;
            movie.DurationMinutes = request.DurationMinutes;
            movie.ReleaseDate = request.ReleaseDate;
            movie.Language = request.Language;
            movie.Genre = request.Genre;

            await _movieRepository.UpdateAsync(movie);

            return MapToResponse(movie);
        }

        // Delete movie
        public async Task<bool> DeleteMovieAsync(int movieId)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);

            if (movie == null)
                return false;

            await _movieRepository.DeleteAsync(movie);

            return true;
        }

        // Search movies by title
        public async Task<List<MovieResponse>> SearchMoviesByTitleAsync(
            string title)
        {
            var movies = await _movieRepository.SearchByTitleAsync(title);

            return movies.Select(MapToResponse).ToList();
        }

        // Convert Movie entity into MovieResponse DTO
        private static MovieResponse MapToResponse(Movie movie)
        {
            return new MovieResponse
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                Genre = movie.Genre
            };
        }
    }
}