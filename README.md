# AgriEnergyConnect (Prototype) — .NET 8

This project implements the **Part 2 requirements of the POE (Agri-Energy Connect)**.

---

## ✅ Features implemented according to the rubric

- **Authentication & Authorization**
  - ASP.NET Core Identity with two roles: **Farmer** and **Employee**
  - Separate login flow for each role (validated role at login)
- **Farmer**
  - Manage their own profile (created automatically when employee adds them)
  - Add new products
  - View their own list of products
- **Employee**
  - Add new farmer profiles (also creates Identity user with Farmer role, default password `Farmer@1234`)
  - View all products of any farmer
  - Filter products by category and date range
- **Database**
  - SQL Server connection string in `appsettings.json`
  - EF Core `DbContext` and entities (`Farmer`, `Product`, `ApplicationUser`)
  - Automatic seeding of roles, users, farmers, and products at startup
  - A full SQL script (`DatabaseScript.sql`) is included for manual database creation if needed
- **UI / UX**
  - Fully responsive Bootstrap 5 design with **Bootstrap Icons**
  - Professional navigation bar, footer, and consistent color theme
  - Custom error page (`Views/Shared/Error.cshtml`)
  - **Dark mode toggle** (button bottom-right, preference saved to localStorage)

---

## 👤 Seeded Accounts

- **Employee**  
  Email: `employee@agri.com`  
  Password: `Employee@1234`  

- **Farmer**  
  Email: `farmer@agri.com`  
  Password: `Farmer@1234`  

---

## ▶️ How to Run

1. Install [.NET SDK 8.0.20](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Restore & run the project:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```
3. The database schema is created automatically with `EnsureCreated()` and seeded.
4. Navigate to the URL displayed in console (usually https://localhost:5001).

---

## 🗄️ Database Script

If you prefer to create and seed the database manually (e.g. directly in Azure SQL), use the provided script:  

- File: **DatabaseScript.sql**  
- It contains:
  - Identity tables (AspNetUsers, AspNetRoles, AspNetUserRoles)
  - Domain tables (Farmers, Products)
  - Role seeds (Farmer, Employee)
  - User seeds (Farmer and Employee)
  - Farmer profile + sample products

---

## 🎨 UI Notes

- Navbar with icons and role-based login buttons
- Home page hero with call-to-action buttons
- Farmer dashboard with product list and add form
- Employee dashboard with farmer list, add farmer, and product filters
- Error page styled with red card
- Dark mode support with toggle button

---

## 📖 Rubric Mapping

- **Identity & Authentication** — Implemented with ASP.NET Identity ✔️  
- **Separate Login for Farmer/Employee** — Role validation at login ✔️  
- **Farmer Product Management** — Add/list products ✔️  
- **Employee Management** — Add farmers + view/filter products ✔️  
- **Database Schema** — EF Core entities, SQL script included ✔️  
- **Seeding** — Roles, users, farmers, and products ✔️  
- **UI/UX** — Bootstrap, icons, dark mode, custom error page ✔️  

---
