using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<ActionResult<BookingResponse>> CreateBooking(
            CreateBookingRequest request)
        {
            try
            {
                var booking = await _bookingService.CreateBookingAsync(request);

                return CreatedAtAction(
                    nameof(GetBookingById),
                    new { id = booking.BookingId },
                    booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET: api/bookings
        // GET: api/bookings?showId=1&status=Confirmed
        [HttpGet]
        public async Task<ActionResult<List<BookingResponse>>> GetAllBookings(
            [FromQuery] int? showId,
            [FromQuery] string? status)
        {
            var bookings = await _bookingService.GetAllBookingsAsync(showId, status);

            return Ok(bookings);
        }

        // GET: api/bookings/1
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingResponse>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            return Ok(booking);
        }

        // PUT: api/bookings/1/cancel
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<BookingResponse>> CancelBooking(int id)
        {
            try
            {
                var booking = await _bookingService.CancelBookingAsync(id);

                if (booking == null)
                    return NotFound(new { message = "Booking not found." });

                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}