USE master;
GO

-- 1. Create Database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DeviceDb')
BEGIN
    CREATE DATABASE DeviceDb;
END
GO

USE DeviceDb;
GO

-- 2. Create Table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Devices]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Devices] (
        [Id]           INT            IDENTITY (1, 1) NOT NULL,
        [Name]         NVARCHAR (100) NOT NULL,
        [Manufacturer] NVARCHAR (100) NOT NULL,
        [Type]         NVARCHAR (50)  NOT NULL,
        [Operating System] NVARCHAR (100) NOT NULL,
        [OS version]   NVARCHAR (100) NOT NULL,
        [Processor]    NVARCHAR (100) NOT NULL,
        [RAM amount]   NVARCHAR (100) NOT NULL,
        [Description]  NVARCHAR (200) NOT NULL,
        [Status]       NVARCHAR (20)  DEFAULT 'Available' NOT NULL,
        [DateAdded]    DATETIME       DEFAULT GETUTCDATE() NOT NULL,
        
        CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED ([Id] ASC),
    );
END
GO