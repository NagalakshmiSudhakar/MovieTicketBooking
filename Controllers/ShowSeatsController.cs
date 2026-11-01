
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/shows/{showId}/seats")]
    [ApiController]
    public class ShowSeatsController : ControllerBase
    {
        private readonly IShowSeatService _showSeatService;

        public ShowSeatsController(IShowSeatService showSeatService)
        {
            _showSeatService = showSeatService;
        }

        // GET: api/shows/{showId}/seats
        [HttpGet]
        public async Task<IActionResult> GetSeatsByShowId(int showId)
        {
            try
            {
                var seats = await _showSeatService.GetSeatsByShowIdAsync(showId);
                return Ok(seats);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET: api/shows/{showId}/seats/available
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSeats(int showId)
        {
            try
            {
                var seats = await _showSeatService.GetAvailableSeatsAsync(showId);
                return Ok(seats);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET: api/shows/{showId}/seats/booked
        [HttpGet("booked")]
        public async Task<IActionResult> GetBookedSeats(int showId)
        {
            try
            {
                var seats = await _showSeatService.GetBookedSeatsAsync(showId);
                return Ok(seats);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
