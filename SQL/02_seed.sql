
INSERT INTO Roles(Name) VALUES ('Farmer'), ('Employee');

-- Demo users
INSERT INTO Users(FullName, Email, PasswordHash) VALUES
('Demo Farmer','farmer@demo.com','$2a$11$e0NRuWm3mXvLm1s5VHIbZ.OKq6d3z0r8s9Z9u9rD3Gk4n8f3k7v1W'), -- password: Test@123
('Demo Employee','employee@demo.com','$2a$11$e0NRuWm3mXvLm1s5VHIbZ.OKq6d3z0r8s9Z9u9rD3Gk4n8f3k7v1W'); -- password: Test@123

-- Map roles
INSERT INTO UserRoles(UserId, RoleId)
SELECT 1, (SELECT RoleId FROM Roles WHERE Name='Farmer')
UNION ALL
SELECT 2, (SELECT RoleId FROM Roles WHERE Name='Employee');

-- Farmers
INSERT INTO Farmers(Name, Email, Phone, Location) VALUES
('Demo Farmer','farmer@demo.com','+27 11 555 0100','Free State'),
('Green Acres','contact@greenacres.co.za','+27 21 555 1000','Western Cape');

-- Products
INSERT INTO Products(FarmerId, Name, Category, ProductionDate) VALUES
(1,'Solar-Powered Pump','Equipment','2025-02-15'),
(1,'Organic Maize','Crops','2025-03-03'),
(2,'Wind Turbine Blades','Equipment','2025-01-28');
