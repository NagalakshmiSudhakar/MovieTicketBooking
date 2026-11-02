using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        // POST: api/seats
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSeat(
            [FromBody] CreateSeatRequest request)
        {
            try
            {
                var seat = await _seatService.CreateSeatAsync(request);

                return CreatedAtAction(
                    nameof(GetSeatById),
                    new { id = seat.SeatId },
                    seat);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // POST: api/seats/bulk
        [HttpPost("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BulkCreateSeats(
            [FromBody] BulkCreateSeatsRequest request)
        {
            try
            {
                var seats = await _seatService.BulkCreateSeatsAsync(request);

                return Ok(seats);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET: api/seats/screen/1
        [HttpGet("screen/{screenId}")]

        public async Task<IActionResult> GetSeatsByScreenId(int screenId)
        {
            if (screenId <= 0)
            {
                return BadRequest(new { message = "Invalid screen ID." });
            }

            var seats = await _seatService.GetSeatsByScreenIdAsync(screenId);

            return Ok(seats);
        }

        // GET: api/seats/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatById(int id)
        {
            var seat = await _seatService.GetSeatByIdAsync(id);

            if (seat == null)
            {
                return NotFound(new { message = "Seat not found." });
            }

            return Ok(seat);
        }

        // PUT: api/seats/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSeat(
            int id,
            [FromBody] UpdateSeatRequest request)
        {
            try
            {
                var seat = await _seatService.UpdateSeatAsync(id, request);

                if (seat == null)
                {
                    return NotFound(new { message = "Seat not found." });
                }

                return Ok(seat);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/seats/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            bool deleted = await _seatService.DeleteSeatAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Seat not found." });
            }

            return NoContent();
        }
    }
}