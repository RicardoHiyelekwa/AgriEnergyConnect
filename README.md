
# Agri-Energy Connect (ASP.NET Core MVC, Azure SQL)

## Prerequisites
- .NET 8 SDK
- SQL Server Management Studio (SSMS) connected to Azure SQL Database
- Azure SQL Server + Database (create "AgriEnergyConnect")

## 1) Azure SQL via SSMS
1. Connect to your Azure SQL Server in SSMS.
2. Run `01_schema.sql` then `02_seed.sql` in the `SQL` folder.
3. Confirm tables and data exist.

## 2) Configure connection string
Edit `appsettings.json` and set `DefaultConnection` to your Azure SQL connection string.

## 3) Run the MVC app
```
cd AgriEnergyConnect
dotnet restore
dotnet run
```
Browse to https://localhost:5001

### Demo credentials
- Farmer: farmer@demo.com / Test@123
- Employee: employee@demo.com / Test@123

## Roles and Features
- Farmer: add products, view own items (by using their FarmerId which matches seeded user for demo)
- Employee: add farmers, view/filter products for a given farmer

## Notes
- Authentication is cookie-based with BCrypt password hashing.
- Data access uses Dapper for simplicity.
- Filtering available on product list by date range and category.
- Clean styling in `wwwroot/css/site.css`.

## Diagrams
See `Diagrams/ERD.png` and `Diagrams/Architecture.png`.
