using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        // Create one seat
        public async Task<SeatResponse> CreateSeatAsync(
            CreateSeatRequest request)
        {
            bool exists = await _seatRepository.ExistsAsync(
                request.ScreenId,
                request.RowName,
                request.SeatNumber);

            if (exists)
            {
                throw new InvalidOperationException(
                    "A seat with the same row and seat number already exists in this screen.");
            }

            var seat = new Seat
            {
                ScreenId = request.ScreenId,
                RowName = request.RowName,
                SeatNumber = request.SeatNumber,
                SeatType = request.SeatType
            };

            var createdSeat = await _seatRepository.AddAsync(seat);

            return MapToResponse(createdSeat);
        }

        // Bulk create seats
        public async Task<List<SeatResponse>> BulkCreateSeatsAsync(
            BulkCreateSeatsRequest request)
        {
            var duplicateSeats = request.Seats
                .GroupBy(s => new
                {
                    s.RowName,
                    s.SeatNumber
                })
                .Any(g => g.Count() > 1);

            if (duplicateSeats)
            {
                throw new InvalidOperationException(
                    "Duplicate seat coordinates found in the request.");
            }

            foreach (var seat in request.Seats)
            {
                bool exists = await _seatRepository.ExistsAsync(
                    request.ScreenId,
                    seat.RowName,
                    seat.SeatNumber);

                if (exists)
                {
                    throw new InvalidOperationException(
                        $"Seat {seat.RowName}{seat.SeatNumber} already exists in this screen.");
                }
            }

            var seats = request.Seats.Select(s => new Seat
            {
                ScreenId = request.ScreenId,
                RowName = s.RowName,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType
            }).ToList();

            var createdSeats = await _seatRepository.AddRangeAsync(seats);

            return createdSeats.Select(MapToResponse).ToList();
        }

        // Get all seats for a screen
        public async Task<List<SeatResponse>> GetSeatsByScreenIdAsync(
            int screenId)
        {
            var seats = await _seatRepository.GetByScreenIdAsync(screenId);

            return seats.Select(MapToResponse).ToList();
        }

        // Get seat by ID
        public async Task<SeatResponse?> GetSeatByIdAsync(int seatId)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            return seat == null ? null : MapToResponse(seat);
        }

        // Update seat
        public async Task<SeatResponse?> UpdateSeatAsync(
            int seatId,
            UpdateSeatRequest request)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            if (seat == null)
            {
                return null;
            }

            bool duplicateExists = await _seatRepository.ExistsAsync(
                request.ScreenId,
                request.RowName,
                request.SeatNumber);

            if (duplicateExists &&
                (seat.ScreenId != request.ScreenId ||
                 seat.RowName != request.RowName ||
                 seat.SeatNumber != request.SeatNumber))
            {
                throw new InvalidOperationException(
                    "A seat with the same row and seat number already exists in this screen.");
            }

            seat.ScreenId = request.ScreenId;
            seat.RowName = request.RowName;
            seat.SeatNumber = request.SeatNumber;
            seat.SeatType = request.SeatType;

            await _seatRepository.UpdateAsync(seat);

            return MapToResponse(seat);
        }

        // Delete seat
        public async Task<bool> DeleteSeatAsync(int seatId)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            if (seat == null)
            {
                return false;
            }

            await _seatRepository.DeleteAsync(seat);

            return true;
        }

        // Map entity to response DTO
        private static SeatResponse MapToResponse(Seat seat)
        {
            return new SeatResponse
            {
                SeatId = seat.SeatId,
                ScreenId = seat.ScreenId,
                RowName = seat.RowName,
                SeatNumber = seat.SeatNumber,
                SeatType = seat.SeatType
            };
        }
    }
}