# Theatre Module – Implementation Documentation

## 1. Objective

Implemented Theatre Management functionality in the Movie Ticket Booking System using ASP.NET Core Web API, Entity Framework Core and SQL Server.

## 2. Database Configuration

Created the `Theatre` entity with the following properties:

| Property | Description |
|---|---|
| TheatreId | Primary key, Identity |
| Name | Required, maximum 150 characters |
| Location | Required, maximum 200 characters |

Configured the entity using Fluent API in `ApplicationDbContext`.

Created and applied the `CreateTheatreTable` migration.

## 3. Implementation Details

**DTOs**
- CreateTheatreRequest
- UpdateTheatreRequest
- TheatreResponse

**Repository**
- ITheatreRepository
- TheatreRepository

Implemented database operations for adding, retrieving, updating and deleting theatres.

**Service**
- ITheatreService
- TheatreService

Implemented business logic and DTO-to-entity mapping.

**Controller**
- TheatresController

Implemented REST API endpoints for Theatre management.

## 4. API Endpoints

| HTTP Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/theatres` | Create theatre |
| GET | `/api/theatres` | Retrieve all theatres |
| GET | `/api/theatres/{id}` | Retrieve theatre by ID |
| PUT | `/api/theatres/{id}` | Update theatre |
| DELETE | `/api/theatres/{id}` | Delete theatre |

## 5. Dependency Injection

Registered the following services in `Program.cs`:

- ITheatreRepository → TheatreRepository
- ITheatreService → TheatreService

## 6. Testing

Tested all five Theatre API endpoints successfully using Postman.

Verified successful creation, retrieval, updating and deletion of theatre records.

## 7. Git Commit

**Commit message:**

`feat(theatres): implement theatre CRUD APIs`

## 8. Result

The Theatre Management module was successfully implemented and tested. The application can now manage theatre records through REST API endpoints.
