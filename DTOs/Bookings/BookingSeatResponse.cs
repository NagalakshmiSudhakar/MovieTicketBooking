
namespace MovieTicketBooking.DTOs.Bookings
{
    public class BookingSeatResponse
    {
        public int SeatId { get; set; }

        public string SeatLabel { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}

