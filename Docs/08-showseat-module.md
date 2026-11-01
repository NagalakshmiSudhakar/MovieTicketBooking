# ShowSeat Module

## Overview
The ShowSeat module manages seat availability for individual movie shows.

## Features
- Retrieve the complete seat layout for a show.
- Retrieve available seats.
- Retrieve booked seats.
- Validate show existence.
- Return seat labels, seat types, ticket prices and availability status.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/shows/{showId}/seats` | Get all seats |
| GET | `/api/shows/{showId}/seats/available` | Get available seats |
| GET | `/api/shows/{showId}/seats/booked` | Get booked seats |

## Architecture
Controller → Service → Repository → ApplicationDbContext

## Business Logic
- Seat labels are generated using RowName and SeatNumber.
- Ticket price is retrieved from the associated Show.
- Available and booked seats are filtered using their Status.
- Invalid ShowIds return 404 Not Found.

## Testing
Tested using Postman and SQL Server.

## Dependencies
- Show
- Seat
- ShowSeat
- Entity Framework Core
- SQL Server