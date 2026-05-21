# Smart Asset Tracking System (work in progress)
Small console application to track company assets (computers and phones). 
The app stores simple asset records, calculates end-of-life (EoL) and prints a formatted asset list with local and converted prices.
This project is an improvement of a previous project called ["WeeklyProject03_AssetTracking"](https://github.com/jiantaoshen/WeeklyProject03_AssetTracking).

## Key Changes from the previous version
- Changed from Clean Architecture to Layered Architecture (N-tier) with Separation of Concerns (SoC)
- Pass only the data the service actually needs instead of the entire asset object, to reduce coupling and improve clarity.
- Use Entity Framework Core with an SQL local database instead of hardcoded sample data, to allow for more realistic data management.

## Set up
1. Clone the repository and open the solution in Visual Studio.
2. Install follow Nuget packages in Visual Studio:
	- Microsoft.EntityFrameworkCore 10.0.8
	- Microsoft.EntityFrameworkCore.Tools 10.0.8
	- Microsoft.EntityFrameworkCore.Design 10.0.8
	- Microsoft.EntityFrameworkCore.sqlServer 10.0.8
3. Install SQL Server Developer Edition or use an existing SQL Server instance.
4. Install SQL Server Management Studio (SSMS) in Visual Studio Installer for database management.
5. Run `add-migration init-table-creation` in the Package Manager Console to create the initial migration for the database schema.
6. Run `update-database` in the Package Manager Console to apply the migration and create the database and tables.
7. Run the console application to start adding assets and viewing the list.

## Features
- Add assets via interactive console input
- User input local price based on the office location (USD for US, SEK for Sweden, EUR for Germany) and it will be converted to USD (exchange rate based on current exchange rates) 
- Show 20 assets (Pagingation, could be changed in AssetService.cs) with EoL status (indicated by color coding, yellow for 6 months remaining, red for 3 months remaining and expired)
- Menu navigation with options to add/remove/update/search assets, view assets, and quit the application
- Database `Assets` with discriminator `ComputerAsset` and `MobileAsset`
- Sample assets are inserted at startup for quick inspection


## Tech stack
- C# 14
- .NET 10
- Entity Framework Core 10
- SQL Server

## Project structure (high level)
- `Program.cs` — Entry point
- `Data/MyDbContext.cs` — Database context for Entity Framework Core
- `Models/Assets.cs` — Asset models: `Asset`, `ComputerAsset`, `MobileAsset`

## Future work
... (work in progress)