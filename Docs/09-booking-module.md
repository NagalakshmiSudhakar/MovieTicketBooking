# Booking Module

## Overview

The Booking module allows customers to book available seats for a scheduled movie show. It validates seat availability, calculates the total ticket price on the server, creates booking records, and updates seat status.

## Components

| Component | Responsibility |
|---|---|
| Booking | Stores customer and booking details |
| BookingSeat | Stores seats associated with a booking |
| CreateBookingRequest | Accepts customer, show, and seat details |
| BookingResponse | Returns booking information |
| IBookingRepository | Defines booking data operations |
| BookingRepository | Implements database operations |
| IBookingService | Defines booking business operations |
| BookingService | Handles booking and cancellation logic |
| BookingsController | Exposes booking REST APIs |

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/bookings` | Create a booking |
| GET | `/api/bookings` | Retrieve all bookings |
| GET | `/api/bookings?showId=1` | Filter bookings by show |
| GET | `/api/bookings?status=Confirmed` | Filter bookings by status |
| GET | `/api/bookings/{id}` | Retrieve a booking |
| PUT | `/api/bookings/{id}/cancel` | Cancel a booking |

## Booking Workflow

1. Receive the customer details, ShowId, and selected SeatIds.
2. Validate the request and reject duplicate seat IDs.
3. Verify that the requested show exists.
4. Verify that every selected seat belongs to the requested show.
5. Check that all selected seats are available.
6. Calculate the total amount using the show's ticket price.
7. Create the Booking record.
8. Create the associated BookingSeat records.
9. Update the selected ShowSeats to `Booked`.
10. Commit all database changes in a transaction.

## Cancellation Workflow

1. Retrieve the booking.
2. Verify that the booking exists and has `Confirmed` status.
3. Retrieve its associated booking seats.
4. Change the corresponding ShowSeats status to `Available`.
5. Update the booking status to `Cancelled`.
6. Commit the changes in a database transaction.

Booking and BookingSeat records are preserved after cancellation for history.

## Business Rules

- A customer must select at least one seat.
- Duplicate seat IDs are not allowed in a request.
- Seats must belong to the selected show.
- Only available seats can be booked.
- Ticket prices are calculated using the show's ticket price.
- Booking creation updates booking records and seat availability in one transaction.
- Only confirmed bookings can be cancelled.
- Cancellation preserves booking history and releases the selected seats.

## Error Handling

| HTTP Status | Scenario |
|---|---|
| 201 Created | Booking created successfully |
| 200 OK | Booking retrieved or cancelled |
| 404 Not Found | Show or booking not found |
| 409 Conflict | Selected seats are unavailable or booking cannot be cancelled |

## Database Tables

- `Bookings`
- `BookingSeats`
- `ShowSeats`

## Testing

Tested through Postman:

- Create a booking.
- Retrieve all bookings.
- Retrieve a booking by ID.
- Filter bookings by show and status.
- Verify booked seat status.
- Reject unavailable seats.
- Cancel a confirmed booking.
- Verify released seat availability.
- Verify cancelled booking history.

## Limitations

The current V1 implementation uses basic availability validation and database transactions. It does not implement advanced concurrency protection for simultaneous booking requests.
