# Seat Module

## Overview
The Seat module manages physical seats belonging to screens in the Movie Ticket Booking System.

## Database Structure
- `SeatId` – Primary key.
- `ScreenId` – Foreign key referencing Screens.
- `RowName` – Seat row identifier.
- `SeatNumber` – Positive seat number.
- `SeatType` – Regular or Premium.

A unique constraint on `ScreenId`, `RowName`, and `SeatNumber` prevents duplicate seat coordinates within a screen.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/seats` | Create a seat |
| POST | `/api/seats/bulk` | Create multiple seats |
| GET | `/api/seats/screen/{screenId}` | Get seats by screen |
| GET | `/api/seats/{id}` | Get seat by ID |
| PUT | `/api/seats/{id}` | Update a seat |
| DELETE | `/api/seats/{id}` | Delete a seat |

## Architecture
- **Model:** Seat entity
- **DTOs:** CreateSeatRequest, BulkCreateSeatsRequest, UpdateSeatRequest, SeatResponse
- **Repository:** Handles database operations
- **Service:** Handles business logic and duplicate-seat validation
- **Controller:** Exposes REST API endpoints

## Validation
- Row name is limited to 5 characters.
- Seat number must be greater than zero.
- Seat type is limited to 30 characters.
- Duplicate seat coordinates are rejected.
- ScreenId must reference an existing screen.

## Git Commit
`feat(seats): implement seat management APIs`
