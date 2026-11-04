using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MovieTicketBooking.Models;
using MovieTicketBooking.Authorization.Requirements;

namespace MovieTicketBooking.Authorization.Handlers
{
    public class BookingOwnerHandler
        : AuthorizationHandler<BookingOwnerRequirement, Booking>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            BookingOwnerRequirement requirement,
            Booking booking)
        {
            // Get the logged-in user's ID from JWT
            var userId = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            // Check whether the logged-in user owns this booking
            if (userId != null && userId == booking.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}