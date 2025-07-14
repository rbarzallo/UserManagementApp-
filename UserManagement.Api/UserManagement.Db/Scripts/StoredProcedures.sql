-- UserManagement.Db/Scripts/StoredProcedures.sql

USE UserManagement;
GO

-- Registrar usuario
IF OBJECT_ID('usp_RegisterUser', 'P') IS NOT NULL
    DROP PROCEDURE usp_RegisterUser;
GO
CREATE PROCEDURE usp_RegisterUser
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(MAX),
    @TwoFactorSecret NVARCHAR(100) = NULL
AS
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, TwoFactorSecret)
    VALUES (@Username, @Email, @PasswordHash, @TwoFactorSecret)
END
GO

-- Obtener usuario por email
IF OBJECT_ID('usp_GetUserByEmail', 'P') IS NOT NULL
    DROP PROCEDURE usp_GetUserByEmail;
GO
CREATE PROCEDURE usp_GetUserByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Users WHERE Email = @Email
END
GO

-- Activar/desactivar doble autenticación
IF OBJECT_ID('usp_SetTwoFactorEnabled', 'P') IS NOT NULL
    DROP PROCEDURE usp_SetTwoFactorEnabled;
GO
CREATE PROCEDURE usp_SetTwoFactorEnabled
    @Email NVARCHAR(100),
    @Enabled BIT
AS
BEGIN
    UPDATE Users
    SET TwoFactorEnabled = @Enabled
    WHERE Email = @Email
END
GO

-- Actualizar secreto de doble factor
IF OBJECT_ID('usp_UpdateTwoFactorSecret', 'P') IS NOT NULL
    DROP PROCEDURE usp_UpdateTwoFactorSecret;
GO
CREATE PROCEDURE usp_UpdateTwoFactorSecret
    @Email NVARCHAR(100),
    @TwoFactorSecret NVARCHAR(100)
AS
BEGIN
    UPDATE Users
    SET TwoFactorSecret = @TwoFactorSecret
    WHERE Email = @Email
END
GO

-- Eliminar usuario
IF OBJECT_ID('usp_DeleteUser', 'P') IS NOT NULL
    DROP PROCEDURE usp_DeleteUser;
GO
CREATE PROCEDURE usp_DeleteUser
    @Email NVARCHAR(100)
AS
BEGIN
    DELETE FROM Users WHERE Email = @Email
END
GO
