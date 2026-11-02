using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // POST: api/movies
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMovie(
            CreateMovieRequest request)
        {
            var movie = await _movieService.CreateMovieAsync(request);

            return CreatedAtAction(
                nameof(GetMovieById),
                new { id = movie.MovieId },
                movie);
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();

            return Ok(movies);
        }

        // GET: api/movies/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found." });

            return Ok(movie);
        }

        // PUT: api/movies/1
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovie(
            int id, UpdateMovieRequest request)
        {
            var movie = await _movieService.UpdateMovieAsync(id, request);

            if (movie == null)
                return NotFound(new { message = "Movie not found." });

            return Ok(movie);
        }

        // DELETE: api/movies/1
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.DeleteMovieAsync(id);

            if (!result)
                return NotFound(new { message = "Movie not found." });

            return NoContent();
        }

        // GET: api/movies/search?title=Leo
        [HttpGet("search")]
        public async Task<IActionResult> SearchMoviesByTitle(
            [FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest(new { message = "Movie title is required." });

            var movies = await _movieService.SearchMoviesByTitleAsync(title);

            return Ok(movies);
        }
    }
}