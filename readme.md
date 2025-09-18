# InventoryShop API

API for managing store products, orders, and users.

## Project Description
**InventoryShop** is a learning project built on ASP.NET Core using EF Core and JWT authentication.  
The project implements CRUD operations for products, orders, and users, supports unit testing, and provides a structure for future feature expansion.

## Project Structure

InventoryShop.sln # Solution file
src/ # Main API project
tests/ # Unit tests
.gitignore # Ignored files
README.md # This file


### src/
- `InventoryShop.Api/` — Web API project
  - `Program.cs` — entry point
  - `appsettings.json` — application settings
  - `Properties/launchSettings.json` — launch configuration
  - `bin/` and `obj/` — auto-generated build folders
  - Project entities, DTOs, controllers, and services

### tests/
- `InventoryShop.Tests/` — unit tests
  - Unit tests for services and controllers

## Technology Stack
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core (SQLite)
- JWT Authentication
- xUnit for testing
- Git + GitHub

## Getting Started

1. Clone the repository:
```bash
git clone <URL_REPO>
cd InventoryShop
Build the project:


dotnet build
Run the API:


dotnet run --project src/InventoryShop.Api
Run tests:

dotnet test tests/InventoryShop.Tests
Step-by-Step Development Cheat Sheet
Create the solution:

dotnet new sln -n InventoryShop
Create projects:

dotnet new webapi -o src/InventoryShop.Api
dotnet new xunit -o tests/InventoryShop.Tests
Add projects to the solution:

dotnet sln add src/InventoryShop.Api/InventoryShop.Api.csproj
dotnet sln add tests/InventoryShop.Tests/InventoryShop.Tests.csproj
Add a reference to the API project in tests:

cd tests/InventoryShop.Tests
dotnet add reference ../../src/InventoryShop.Api/InventoryShop.Api.csproj
Configure .gitignore for Visual Studio and C#

Create an empty Program.cs and set up DI, services, logging

Create entities and tables using EF Core

Implement CRUD and HTTP endpoints

Add unit tests in parallel

Create feature branches for each feature and commit with meaningful messages

Branches and Pull Requests
main — main branch

feature/<name> — new features