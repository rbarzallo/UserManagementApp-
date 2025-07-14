-- UserManagement.Db/Scripts/Tables.sql

USE master;
GO

-- Crear base de datos si no existe
IF NOT EXISTS (
    SELECT name 
    FROM sys.databases 
    WHERE name = N'UserManagement'
)
CREATE DATABASE UserManagement;
GO

USE UserManagement;
GO

-- Crear tabla Users
IF NOT EXISTS (
    SELECT * 
    FROM sysobjects 
    WHERE name='Users' AND xtype='U'
)
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    TwoFactorEnabled BIT DEFAULT 0,
    TwoFactorSecret NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO
