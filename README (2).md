
# 📘 README.md — AgriEnergyConnect

## 🌱 Project Overview
Agri-Energy Connect is a prototype web application built with **ASP.NET Core MVC** and **Azure SQL Database**.  
It enables **Farmers** and **Employees** to collaborate in managing agricultural products and sustainable energy solutions.

This prototype was developed for the **Enterprise Application Development (EAPD7111)** module at **Rosebank College**.

---

## ⚙️ Features
- **Authentication & Roles**
  - Farmers → register/login, create their own farmer profile, and add products.
  - Employees → register/login, add new farmers, view/filter farmers' products.
- **Product Management**
  - Farmers can add products (name, category, production date).
  - Employees can add products to any farmer and filter by category or date.
- **Validation & Security**
  - Passwords encrypted with **BCrypt**.
  - Role-based access control.
  - Server-side validation of forms.
- **Responsive UI**
  - Views styled with **Bootstrap 5** for consistency across devices.

---

## 🗄 Database Setup (Azure SQL with SSMS)

1. Open **SQL Server Management Studio (SSMS)**.
2. Connect to your **Azure SQL Database** instance.
3. Run the script file `DatabaseScript.sql` (included in this project).  
   This will create the following tables:
   - `Users`
   - `Farmers`
   - `Products`
4. Verify that the tables and sample seed data were created.

---

## 🔑 Seed Users

| Role      | Email                   | Password  |
|-----------|-------------------------|-----------|
| Employee  | employee@agrienergy.com | `Test@123` (hash must be replaced with BCrypt) |
| Farmer    | farmer@agrienergy.com   | `Test@123` (hash must be replaced with BCrypt) |

⚠️ Note: Replace the hashed password in `DatabaseScript.sql` with a real BCrypt hash generated for `Test@123`.  
You can generate one quickly in C#:
```csharp
var hash = BCrypt.Net.BCrypt.HashPassword("Test@123");
Console.WriteLine(hash);
```

---

## 🔧 Configuration

1. Update `appsettings.json` with your **Azure SQL connection string**:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tcp:YOUR-SERVER.database.windows.net,1433;Initial Catalog=AgriEnergyDB;Persist Security Info=False;User ID=YOUR-USER;Password=YOUR-PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}
```

2. Ensure `DbConfig` is using this connection string in your project.

---

## ▶️ Running the Application

1. Clone or unzip the project into Visual Studio.
2. Ensure that `AgriEnergyConnect.sln` is set as the startup project.
3. Restore NuGet packages.
4. Run the application (`dotnet run` or press F5 in Visual Studio).
5. Navigate to `https://localhost:5001` (or the port Visual Studio assigns).

---

## ✅ Testing the Prototype

1. **Login as Employee**
   - Use: `employee@agrienergy.com` / `Test@123`
   - Add new farmers.
   - View and filter farmers’ products.

2. **Login as Farmer**
   - Use: `farmer@agrienergy.com` / `Test@123`
   - Create your farmer profile.
   - Add products (with category + production date).

3. **Filtering Products**
   - Employee selects a farmer.
   - Apply filters by **date range** and/or **category**.

---

## 📖 Technologies Used
- ASP.NET Core MVC 8.0
- Azure SQL Database
- Dapper ORM
- Bootstrap 5
- BCrypt.Net for password security

---

## 📚 References
- Microsoft Docs: [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- Dapper Documentation: [Dapper GitHub](https://github.com/DapperLib/Dapper)
- BCrypt.Net: [Password Hashing](https://github.com/BcryptNet/bcrypt.net)
