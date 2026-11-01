
namespace MovieTicketBooking.DTOs.Bookings
{
    public class BookingResponse
    {
        public int BookingId { get; set; }

        public string BookingReference { get; set; } = string.Empty;

        public int ShowId { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<BookingSeatResponse> Seats { get; set; } = new();
    }
}
