# ContosoUniversity API

A simple ASP.NET Core Web API based on the classic Contoso University sample.

This project provides a lightweight API for managing university data such as students, courses, departments, instructors, and enrollments using Entity Framework Core with SQLite.

## Features

- ASP.NET Core Web API
- Entity Framework Core with SQLite
- Seeded sample data for the university database
- CRUD-style controllers for core school entities
- OpenAPI support in development

## Tech Stack

- .NET 10
- ASP.NET Core
- Entity Framework Core
- SQLite

## Getting Started

1. Restore dependencies:
   ```bash
   dotnet restore
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the API:
   ```bash
   dotnet run --project ContosoUniversity
   ```

4. Open the app in a browser or API client. The app will show the local URL in the console, and OpenAPI metadata is available in development mode.

## Project Structure

- `ContosoUniversity/Controllers` - API controllers
- `ContosoUniversity/Models` - Entity models
- `ContosoUniversity/Data` - EF Core context and data seeding
- `ContosoUniversity/Migrations` - database migrations
- `ContosoUniversity/Program.cs` - application startup

## Notes

- The database is configured in `appsettings.json` and uses SQLite.
- Sample data is initialized automatically when the app starts.

## License

This project is intended for learning and local development use.
