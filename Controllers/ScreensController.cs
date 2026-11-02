using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreensController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        // POST: api/screens
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ScreenResponse>> CreateScreen(
            CreateScreenRequest request)
        {
            var screen = await _screenService.CreateScreenAsync(request);

            return CreatedAtAction(
                nameof(GetScreenById),
                new { id = screen.ScreenId },
                screen);
        }

        // GET: api/screens
        [HttpGet]
        public async Task<ActionResult<List<ScreenResponse>>> GetAllScreens()
        {
            var screens = await _screenService.GetAllScreensAsync();

            return Ok(screens);
        }

        // GET: api/screens/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ScreenResponse>> GetScreenById(int id)
        {
            var screen = await _screenService.GetScreenByIdAsync(id);

            if (screen == null)
                return NotFound(new { message = "Screen not found." });

            return Ok(screen);
        }

        // GET: api/screens/theatre/1
        [HttpGet("theatre/{theatreId:int}")]
        public async Task<ActionResult<List<ScreenResponse>>> GetScreensByTheatreId(
            int theatreId)
        {
            var screens = await _screenService.GetScreensByTheatreIdAsync(theatreId);

            return Ok(screens);
        }

        // PUT: api/screens/1
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ScreenResponse>> UpdateScreen(
            int id,
            UpdateScreenRequest request)
        {
            var screen = await _screenService.UpdateScreenAsync(id, request);

            if (screen == null)
                return NotFound(new { message = "Screen not found." });

            return Ok(screen);
        }

        // DELETE: api/screens/1
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteScreen(int id)
        {
            var deleted = await _screenService.DeleteScreenAsync(id);

            if (!deleted)
                return NotFound(new { message = "Screen not found." });

            return NoContent();
        }
    }
}