# Smart Asset Tracking System (work in progress)
Small console application to track company assets (computers and phones). 
The app stores simple asset records, calculates end-of-life (EoL) and prints a formatted asset list with local and converted prices.
This project is an improvement of a previous project called ["WeeklyProject03_AssetTracking"](https://github.com/jiantaoshen/WeeklyProject03_AssetTracking).

## Key Changes from the previous version
- Changed from layered architecture to a more straightforward structure, as the project is small and doesn't require complex separation of concerns.
- Pass only the data the service actually needs instead of the entire asset object, to reduce coupling and improve clarity.
- Use Entity Framework Core with an SQL local database instead of hardcoded sample data, to allow for more realistic data management.
- 

## Set up
1. Install follow Nuget packages in Visual Studio:
	- Microsoft.EntityFrameworkCore 10.0.8
	- Microsoft.EntityFrameworkCore.Tools 10.0.8
	- Microsoft.EntityFrameworkCore.Design 10.0.8
	- Microsoft.EntityFrameworkCore.sqlServer 10.0.8
2. Install SQL Server Developer Edition or use an existing SQL Server instance.
3. Install SQL Server Management Studio (SSMS) in Visual Studio Installer for database management.

## Features
- Add assets via interactive console input
- Sample/test assets are inserted at startup for quick inspection
- List assets grouped by office/type with price converted to USD using a currency service
- Simple domain model with `Asset`, `Computer`, and `Phone`

## Tech stack
- C# 14
- .NET 10

## Getting started
The console app will prompt for new assets. Enter `Q` for the office prompt to quit input mode and display the list.

## Usage notes
- Purchase date must be entered as `yyyy-MM-dd`.
- Supported currencies are read from the `CurrencyService` at startup; input must match an available ISO currency code (e.g. `USD`, `EUR`, `SEK`).

## Project structure (high level)
- `Program.cs` — Console UI / entry point
- `Application/Use_Cases` — Use cases: `AddAsset`, `ListAssets`
- `Application/Interfaces` — Port interfaces such as `ICurrencyService` and repository interface
- `Domain/Entities` — Domain models: `Asset`, `Computer`, `Phone`
- `Domain/Services` — Domain-level logic (EoL calculation, validations)
- `Infrastruture` — Implementations: in-memory repository and currency service/converter

## Design
This project follows a layered clean architecture:
- Presentation (console) depends on Application use cases
- Application defines ports and coordinates Domain and Infrastructure
- Domain contains entity and business rules
- Infrastructure implements persistence and external services (currency)

## Future work
It is good enough for a small console app, so I won't add more features.

## AI usage
I created this project by myself, but I used AI to help convert it to clean architecture and to write this README. The code was originally a single file with all logic, and I refactored it into layers with AI assistance. The README was generated based on the project structure and features.
The result is fine and I need to do some manual adjustments to make it more readable and accurate, but overall the AI provided a good starting point for both the architecture and documentation.
I also used AI to generate a UML class diagram, but it is bad so I decided to delete it. 
