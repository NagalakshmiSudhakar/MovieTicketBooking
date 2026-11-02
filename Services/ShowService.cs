using MovieTicketBooking.Data;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Exceptions;

namespace MovieTicketBooking.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ApplicationDbContext _context;

        public ShowService(
            IShowRepository showRepository,
            ISeatRepository seatRepository,
            ApplicationDbContext context)
        {
            _showRepository = showRepository;
            _seatRepository = seatRepository;
            _context = context;
        }

        // Create show and generate ShowSeats
        public async Task<ShowResponse> CreateShowAsync(
            CreateShowRequest request)
        {
            if (request.EndTime <= request.StartTime)
            {
                throw new BadRequestException(
                    "End time must be later than start time.");
            }

            bool hasOverlap =
                await _showRepository.HasOverlappingShowAsync(
                    request.ScreenId,
                    request.StartTime,
                    request.EndTime);

            if (hasOverlap)
            {
                throw new ConflictException(
                    "This screen already has a show scheduled during the selected time.");
            }

            var physicalSeats =
                await _seatRepository.GetByScreenIdAsync(request.ScreenId);

            if (physicalSeats.Count == 0)
            {
                throw new BadRequestException(
                    "No physical seats are configured for this screen.");
            }

            var show = new Show
            {
                MovieId = request.MovieId,
                ScreenId = request.ScreenId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                TicketPrice = request.TicketPrice
            };

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var createdShow = await _showRepository.AddAsync(show);

                var showSeats = physicalSeats.Select(seat =>
                    new ShowSeat
                    {
                        ShowId = createdShow.ShowId,
                        SeatId = seat.SeatId,
                        Status = "Available"
                    }).ToList();

                await _context.ShowSeats.AddRangeAsync(showSeats);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapToResponse(createdShow);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Get all shows with optional filters
        public async Task<List<ShowResponse>> GetAllShowsAsync(
            DateTime? date,
            int? movieId,
            int? theatreId)
        {
            var shows = await _showRepository.GetAllAsync(
                date, movieId, theatreId);

            return shows.Select(MapToResponse).ToList();
        }

        // Get show by ID
        public async Task<ShowResponse?> GetShowByIdAsync(int showId)
        {
            var show = await _showRepository.GetByIdAsync(showId);

            return show == null ? null : MapToResponse(show);
        }

        // Get shows by movie
        public async Task<List<ShowResponse>> GetShowsByMovieIdAsync(
            int movieId)
        {
            var shows =
                await _showRepository.GetByMovieIdAsync(movieId);

            return shows.Select(MapToResponse).ToList();
        }

        // Get shows by theatre
        public async Task<List<ShowResponse>> GetShowsByTheatreIdAsync(
            int theatreId)
        {
            var shows =
                await _showRepository.GetByTheatreIdAsync(theatreId);

            return shows.Select(MapToResponse).ToList();
        }

        // Get shows by date
        public async Task<List<ShowResponse>> GetShowsByDateAsync(
            DateTime date)
        {
            var shows =
                await _showRepository.GetByDateAsync(date);

            return shows.Select(MapToResponse).ToList();
        }

        // Update show
        public async Task<ShowResponse?> UpdateShowAsync(
            int showId,
            UpdateShowRequest request)
        {
            var show = await _showRepository.GetByIdAsync(showId);

            if (show == null)
            {
                return null;
            }

            if (request.EndTime <= request.StartTime)
            {
                throw new BadRequestException(
                    "End time must be later than start time.");
            }

            bool hasOverlap =
                await _showRepository.HasOverlappingShowAsync(
                    request.ScreenId,
                    request.StartTime,
                    request.EndTime,
                    showId);

            if (hasOverlap)
            {
                throw new ConflictException(
                    "This screen already has a show scheduled during the selected time.");
            }

            show.MovieId = request.MovieId;
            show.ScreenId = request.ScreenId;
            show.StartTime = request.StartTime;
            show.EndTime = request.EndTime;
            show.TicketPrice = request.TicketPrice;

            await _showRepository.UpdateAsync(show);

            return MapToResponse(show);
        }

        // Delete show
        public async Task<bool> DeleteShowAsync(int showId)
        {
            var show = await _showRepository.GetByIdAsync(showId);

            if (show == null)
            {
                return false;
            }

            await _showRepository.DeleteAsync(show);

            return true;
        }

        // Map entity to response DTO
        private static ShowResponse MapToResponse(Show show)
        {
            return new ShowResponse
            {
                ShowId = show.ShowId,
                MovieId = show.MovieId,
                ScreenId = show.ScreenId,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                TicketPrice = show.TicketPrice
            };
        }
    }
}