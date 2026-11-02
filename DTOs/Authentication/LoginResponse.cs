namespace MovieTicketBooking.DTOs.Authentication
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public IList<string> Roles { get; set; } = new List<string>();
    }
}