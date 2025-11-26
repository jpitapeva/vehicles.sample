# Vehicles (experimental)

An experimental API/service for vehicle management.

## Overview
- Purpose: Prototype for CRUD operations over vehicle-related entities (create, read, update).
- Scope: Experimental — suitable for exploring patterns, EF migrations, and local dev workflows.

## Technologies
- .NET 8 (SDK)
- C#
- Entity Framework Core (EF Core)
- SQL Server (local instance or Docker container)
- Recommended tools: Visual Studio, Visual Studio Code, .NET CLI, Docker

## Prerequisites
- .NET 8 SDK installed (check with `dotnet --version`)
- Docker (if you plan to run SQL Server in a container)
- An IDE or text editor (VS, VS Code) or use the .NET CLI

## Database (example with Docker)
You can run SQL Server locally or in a Docker container. Example Docker command (replace password with a secure one):

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name sql_vehicles -d mcr.microsoft.com/mssql/server:2019-latest
```

## Connection string / Local configuration
Update the local configuration file with your SQL Server connection string. The project uses `sharedSettings.Local.json`. Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=VehiclesDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  }
}
```

## Running the project locally

1. Clone the repository:
```bash
git clone <repo-url>
cd <repo-directory>
```

2. Restore packages and build:
```bash
dotnet restore
dotnet build
```

3. Apply EF Core migrations to create or update the database:
- Update database from existing migrations:
```bash
dotnet ef database update --project <Project.Data> --startup-project <Project.ApiOrApp>
```
- Create a new migration after model changes:
```bash
dotnet ef migrations add MigrationName --project <Project.Data> --startup-project <Project.ApiOrApp>
```

4. Run the application

5. Evidences
![Evidences](https://github.com/jpitapeva/vehicles.sample/blob/main/evidences/Captura%20de%20tela%202025-11-12%20224415.png)
![Evidences](https://github.com/jpitapeva/vehicles.sample/blob/main/evidences/Captura%20de%20tela%202025-11-12%20211547.png)
![Evidences](https://github.com/jpitapeva/vehicles.sample/blob/main/evidences/Captura%20de%20tela%202025-11-12%20202614.png)


