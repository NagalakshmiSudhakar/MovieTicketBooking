# Database Configuration

## Objective
Configure Entity Framework Core with SQL Server for the Movie Ticket Booking Web API.

## Changes Made

- Installed EF Core SQL Server, Tools and Design packages.
- Added the SQL Server connection string in `appsettings.json`.
- Created `ApplicationDbContext.cs` inside the `Data` folder.
- Registered `ApplicationDbContext` in `Program.cs` using dependency injection.
- Configured SQL Server using `UseSqlServer()`.

## Error Fixed

- Resolved missing Entity Framework Core namespace errors.
- Fixed `AddDbContext()` recognition issue by installing and restoring the required EF Core packages.

## Verification

- Successfully restored NuGet packages.
- Project build: [Add actual result].

## Git Commit

`chore(database): configure EF Core and SQL Server`

