# Screen Module

## Overview
The Screen module manages screens belonging to theatres in the Movie Ticket Booking System.

## Database
**Table:** `Screens`

| Column | Data Type | Description |
|---|---|---|
| ScreenId | int | Primary key, Identity |
| TheatreId | int | Foreign key referencing Theatres |
| ScreenName | nvarchar(50) | Screen name |

## Implementation

- **Model:** `Screen.cs`
- **DTOs:** `CreateScreenRequest`, `UpdateScreenRequest`, `ScreenResponse`
- **Repository:** `IScreenRepository`, `ScreenRepository`
- **Service:** `IScreenService`, `ScreenService`
- **Controller:** `ScreensController`
- **Database Context:** Screen DbSet and Fluent API configuration in `ApplicationDbContext`

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/screens` | Create a screen |
| GET | `/api/screens` | Retrieve all screens |
| GET | `/api/screens/{id}` | Retrieve screen by ID |
| GET | `/api/screens/theatre/{theatreId}` | Retrieve screens by theatre |
| PUT | `/api/screens/{id}` | Update a screen |
| DELETE | `/api/screens/{id}` | Delete a screen |

## Validation and Relationships

- Each screen belongs to a theatre through `TheatreId`.
- The foreign key maintains the relationship between Theatres and Screens.
- Restrictive delete behavior protects theatre records that have dependent screens.

## Testing

All six Screen API endpoints were tested successfully using Postman:
- Create Screen
- Get All Screens
- Get Screen by ID
- Get Screens by Theatre ID
- Update Screen
- Delete Screen

## Result

The Screen module was implemented, built, and tested successfully.