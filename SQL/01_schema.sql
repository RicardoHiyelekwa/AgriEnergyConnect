-- ======================================
-- Database Schema for AgriEnergyConnect
-- ======================================

DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Farmers;
DROP TABLE IF EXISTS Users;
-- ===================
-- Users Table
-- ===================

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT 'Farmer',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
-- ...


-- ===================
-- Farmers Table
-- ===================
CREATE TABLE Farmers (
    FarmerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    UserId INT NOT NULL,
    CONSTRAINT FK_Farmers_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- ===================
-- Products Table
-- ===================
CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    ProductionDate DATE NOT NULL,
    FarmerId INT NOT NULL,
    CONSTRAINT FK_Products_Farmers FOREIGN KEY (FarmerId) REFERENCES Farmers(FarmerId) ON DELETE CASCADE
);


