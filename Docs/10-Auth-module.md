# Authentication and Authorization

## Overview

Implemented authentication and authorization using ASP.NET Core Identity and JWT (JSON Web Token) in the Movie Ticket Booking System.

The objective is to secure API endpoints, distinguish between Admin and Customer users, and ensure customers can access only their own bookings.

## 1. Technologies Used

- ASP.NET Core Identity
- JWT Bearer Authentication
- Entity Framework Core
- SQL Server
- Role-Based Authorization
- .NET User Secrets

## 2. Database Changes

### ApplicationUser

Created a custom `ApplicationUser` class extending `IdentityUser`.

Additional property:
- `FullName`

Updated `ApplicationDbContext` to inherit from:

`IdentityDbContext<ApplicationUser>`

Added the Identity database tables through EF Core migrations.

Migration:
- `AddIdentityTables`
- `AddBookingUserRelationship`

### Booking Relationship

Added `UserId` to the Booking entity to associate each booking with its authenticated customer.

Configured the relationship between `Booking` and `ApplicationUser` using Fluent API.

## 3. Authentication

Implemented two authentication endpoints.

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new Customer |
| POST | `/api/auth/login` | Authenticate a user and generate JWT |

### Registration

- Validates FullName, Email and Password.
- Checks whether the email already exists.
- Creates the user through ASP.NET Core Identity.
- Automatically assigns the Customer role.
- Passwords are securely hashed by ASP.NET Core Identity.

### Login

- Validates user credentials.
- Verifies passwords using Identity's password verification.
- Retrieves assigned roles.
- Generates a JWT containing user identity and role claims.
- Returns the token, expiration, user ID, email and roles.

## 4. Authorization

Implemented two roles:

- **Admin** – Manages the movie catalogue, theatres, screens, seats and show schedules.
- **Customer** – Registers, logs in, browses available movies and shows, and manages their own bookings.

### Authorization Policies

| Policy | Access |
|---|---|
| AdminOnly | Admin |
| CustomerOnly | Customer |
| AdminOrCustomer | Admin and Customer |

### API Access Rules

| API Module | GET | POST | PUT | DELETE |
|---|---|---|---|---|
| Movies | Public | Admin | Admin | Admin |
| Theatres | Public | Admin | Admin | Admin |
| Screens | Public | Admin | Admin | Admin |
| Seats | Public | Admin | Admin | Admin |
| Shows | Public | Admin | Admin | Admin |
| Bookings | Admin/Customer | Admin/Customer | Admin/Customer | — |

Booking access is additionally restricted by ownership checks in the BookingService.

- Customers can view their own bookings.
- Customers can cancel their own bookings.
- Admins can view and manage all bookings.

## 5. Admin Account Seeding

Implemented automatic Admin account creation during application startup.

The seeding process:

1. Creates Admin and Customer roles if they do not exist.
2. Reads Admin credentials from configuration.
3. Creates the initial Admin account if it does not exist.
4. Assigns the Admin role to the account.

Used .NET User Secrets to store Admin credentials during local development.

Admin credentials are not stored in the source code or committed to GitHub.

## 6. Security Implementation

- JWT Bearer authentication.
- Role-based authorization.
- ASP.NET Core Identity password hashing.
- User ID obtained from authenticated JWT claims.
- Customer booking ownership validation.
- Admin-only access to management operations.
- Admin credentials stored using User Secrets.

## 7. Testing

Verified using Postman:

- Customer registration.
- Customer login.
- Admin login.
- JWT token generation.
- Customer role assignment.
- Admin role assignment.
- Customer blocked from Admin-only operations.
- Public movie browsing.
- Authenticated booking operations.
- Customer booking ownership restrictions.

## 8. Important Files Added or Modified

| File | Purpose |
|---|---|
| `Models/ApplicationUser.cs` | Custom Identity user |
| `DTOs/Authentication/RegisterRequest.cs` | Registration request |
| `DTOs/Authentication/LoginRequest.cs` | Login request |
| `DTOs/Authentication/LoginResponse.cs` | JWT login response |
| `Services/AuthService.cs` | Registration and login logic |
| `Services/Interfaces/IAuthService.cs` | Authentication service interface |
| `Controllers/AuthController.cs` | Authentication endpoints |
| `Data/ApplicationDbContext.cs` | Identity database integration |
| `Program.cs` | Identity, JWT, policies and role seeding |
| `Models/Booking.cs` | User relationship |
| `Services/BookingService.cs` | Booking ownership validation |
| `Controllers/BookingsController.cs` | Booking authorization |

## 9. Git Commit

**Commit message:**

`feat(auth): implement JWT authentication and role-based authorization`

## 10. Project Version Status

- V1 – Initial Functional Backend
- V2 – Concurrency and Seat Holds (Deferred)
- V3 – Authentication and Authorization (Implemented)

