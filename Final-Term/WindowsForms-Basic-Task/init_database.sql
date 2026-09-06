-- init_database.sql
-- T-SQL script to create a database and table for FinalLab-FormSubmission
-- Usage: Run this script in SQL Server Management Studio or sqlcmd. Adjust names as needed.

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'FinalLabFormDB')
BEGIN
    CREATE DATABASE [FinalLabFormDB];
END
GO

USE [FinalLabFormDB];
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Submissions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Submissions]
    (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Username] NVARCHAR(100) NOT NULL,
        [PasswordHash] VARBINARY(64) NOT NULL,
        [Gender] NVARCHAR(10) NULL,
        [GroupName] CHAR(1) NULL,
        [SubmittedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

-- Example: insert a sample record (password hashed using SHA2_256)
INSERT INTO [dbo].[Submissions] ([Username],[PasswordHash],[Gender],[GroupName])
VALUES (
    N'testuser',
    HASHBYTES('SHA2_256', CONVERT(NVARCHAR(4000), N'mypassword')),
    N'Male',
    'A'
);
GO

-- Example select
SELECT TOP 10 [Id],[Username],[Gender],[GroupName],[SubmittedAt]
FROM [dbo].[Submissions]
ORDER BY [SubmittedAt] DESC;
