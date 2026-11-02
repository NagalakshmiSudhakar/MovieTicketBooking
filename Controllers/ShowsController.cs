using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        // POST: api/shows
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateShow(
            [FromBody] CreateShowRequest request)
        {
            try
            {
                var show = await _showService.CreateShowAsync(request);

                return CreatedAtAction(
                    nameof(GetShowById),
                    new { id = show.ShowId },
                    show);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET: api/shows
        [HttpGet]
        public async Task<IActionResult> GetAllShows(
            [FromQuery] DateTime? date,
            [FromQuery] int? movieId,
            [FromQuery] int? theatreId)
        {
            var shows = await _showService.GetAllShowsAsync(
                date, movieId, theatreId);

            return Ok(shows);
        }

        // GET: api/shows/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(int id)
        {
            var show = await _showService.GetShowByIdAsync(id);

            if (show == null)
            {
                return NotFound(new { message = "Show not found." });
            }

            return Ok(show);
        }

        // GET: api/shows/movie/1
        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetShowsByMovieId(int movieId)
        {
            var shows = await _showService.GetShowsByMovieIdAsync(movieId);

            return Ok(shows);
        }

        // GET: api/shows/theatre/1
        [HttpGet("theatre/{theatreId}")]
        public async Task<IActionResult> GetShowsByTheatreId(int theatreId)
        {
            var shows = await _showService.GetShowsByTheatreIdAsync(theatreId);

            return Ok(shows);
        }

        // GET: api/shows/search?date=2026-11-01
        [HttpGet("search")]
        public async Task<IActionResult> GetShowsByDate(
            [FromQuery] DateTime date)
        {
            var shows = await _showService.GetShowsByDateAsync(date);

            return Ok(shows);
        }

        // PUT: api/shows/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateShow(
            int id,
            [FromBody] UpdateShowRequest request)
        {
            try
            {
                var show = await _showService.UpdateShowAsync(id, request);

                if (show == null)
                {
                    return NotFound(new { message = "Show not found." });
                }

                return Ok(show);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/shows/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            bool deleted = await _showService.DeleteShowAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Show not found." });
            }

            return NoContent();
        }
    }
}