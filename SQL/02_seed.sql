-- ===================
-- Sample Seed Data
-- ===================

-- Users (1 Employee, 1 Farmer)
INSERT INTO Users (FullName, Email, PasswordHash, Role)
VALUES
('Admin Employee', 'employee@agrienergy.com', '$2a$11$hashedpassword1234567890', 'Employee'),
('John Farmer', 'farmer@agrienergy.com', '$2a$11$hashedpassword1234567890', 'Farmer');

-- Farmers
INSERT INTO Farmers (Name, Email, Phone, Location, UserId)
VALUES
('John Farmer', 'farmer@agrienergy.com', '0821234567', 'Cape Town, South Africa', 2);

-- Products
INSERT INTO Products (Name, Category, ProductionDate, FarmerId)
VALUES
('Organic Maize', 'Crops', '2025-08-15', 1),
('Solar-Powered Irrigation Pump', 'Equipment', '2025-08-20', 1);