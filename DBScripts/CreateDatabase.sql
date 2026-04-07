USE DeviceDb;
GO

IF OBJECT_ID('[dbo].[Devices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Devices];
GO

CREATE TABLE Devices
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    SerialNumber NVARCHAR(50) NOT NULL UNIQUE,
    Name NVARCHAR(100) NOT NULL,
    Manufacturer NVARCHAR(100) NOT NULL,
    [Type] NVARCHAR(50) NOT NULL,
    OperatingSystem NVARCHAR(50) NOT NULL,
    OsVersion NVARCHAR(50) NOT NULL,
    Processor NVARCHAR(100) NOT NULL,
    RamAmount NVARCHAR(20) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    DateAdded DATETIME DEFAULT GETDATE()
);
GO