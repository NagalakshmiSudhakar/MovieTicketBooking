
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;

        public ScreenService(IScreenRepository screenRepository)
        {
            _screenRepository = screenRepository;
        }

        public async Task<ScreenResponse> CreateScreenAsync(
            CreateScreenRequest request)
        {
            var screen = new MovieTicketBooking.Models.Screen
            {
                TheatreId = request.TheatreId,
                ScreenName = request.ScreenName
            };

            var createdScreen = await _screenRepository.AddAsync(screen);

            return MapToResponse(createdScreen);
        }

        public async Task<List<ScreenResponse>> GetAllScreensAsync()
        {
            var screens = await _screenRepository.GetAllAsync();

            return screens.Select(MapToResponse).ToList();
        }

        public async Task<ScreenResponse?> GetScreenByIdAsync(int screenId)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);

            if (screen == null)
                return null;

            return MapToResponse(screen);
        }

        public async Task<List<ScreenResponse>> GetScreensByTheatreIdAsync(
            int theatreId)
        {
            var screens = await _screenRepository.GetByTheatreIdAsync(theatreId);

            return screens.Select(MapToResponse).ToList();
        }

        public async Task<ScreenResponse?> UpdateScreenAsync(
            int screenId,
            UpdateScreenRequest request)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);

            if (screen == null)
                return null;

            screen.TheatreId = request.TheatreId;
            screen.ScreenName = request.ScreenName;

            await _screenRepository.UpdateAsync(screen);

            return MapToResponse(screen);
        }

        public async Task<bool> DeleteScreenAsync(int screenId)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);

            if (screen == null)
                return false;

            await _screenRepository.DeleteAsync(screen);

            return true;
        }

        private static ScreenResponse MapToResponse(
            MovieTicketBooking.Models.Screen screen)
        {
            return new ScreenResponse
            {
                ScreenId = screen.ScreenId,
                TheatreId = screen.TheatreId,
                ScreenName = screen.ScreenName
            };
        }
    }
}
