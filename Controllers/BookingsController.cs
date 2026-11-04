
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOrCustomer")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IAuthorizationService _authorizationService;

        public BookingsController(
            IBookingService bookingService,
            IBookingRepository bookingRepository,
            IAuthorizationService authorizationService)
        {
            _bookingService = bookingService;
            _bookingRepository = bookingRepository;
            _authorizationService = authorizationService;
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<ActionResult<BookingResponse>> CreateBooking(
            CreateBookingRequest request)
        {
            var booking = await _bookingService.CreateBookingAsync(request);

            return CreatedAtAction(
                nameof(GetBookingById),
                new { id = booking.BookingId },
                booking);
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<ActionResult<List<BookingResponse>>> GetAllBookings(
            [FromQuery] int? showId,
            [FromQuery] string? status)
        {
            var bookings = await _bookingService.GetAllBookingsAsync(
                showId, status);

            return Ok(bookings);
        }

        // GET: api/bookings/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingResponse>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            return Ok(booking);
        }

        // PUT: api/bookings/1/cancel
        [HttpPut("{id:int}/cancel")]
        public async Task<ActionResult<BookingResponse>> CancelBooking(int id)
        {
            // Step 1: Retrieve the booking resource
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            // Step 2: Admin can cancel any booking.
            // Customers must pass the custom ownership policy.
            if (!User.IsInRole("Admin"))
            {
                var authorizationResult =
                    await _authorizationService.AuthorizeAsync(
                        User,
                        booking,
                        "BookingOwner");

                // Step 3: Deny access if the customer does not own the booking
                if (!authorizationResult.Succeeded)
                    return Forbid();
            }

            // Step 4: Cancel only after authorization succeeds
            var result = await _bookingService.CancelBookingAsync(id);

            if (result == null)
                return NotFound(new { message = "Booking not found." });

            return Ok(result);
        }
    }
}
