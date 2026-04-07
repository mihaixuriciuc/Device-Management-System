USE DeviceDb;
GO

-- 1. iPhone 15 Pro
IF NOT EXISTS (SELECT 1
FROM [dbo].[Devices]
WHERE [SerialNumber] = 'SN-APL-P15')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-APL-P15', 'iPhone 15 Pro', 'Apple', 'Phone', 'iOS', '17.4', 'A17 Pro', '8GB', 'Main corporate handheld device.', 'Available');
END

-- 2. Galaxy S24
IF NOT EXISTS (SELECT 1
FROM [dbo].[Devices]
WHERE [SerialNumber] = 'SN-SAM-S24')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-SAM-S24', 'Galaxy S24', 'Samsung', 'Phone', 'Android', '14', 'Snapdragon 8 Gen 3', '12GB', 'Standard Android option for developers.', 'Available');
END

-- 3. iPad Air
IF NOT EXISTS (SELECT 1
FROM [dbo].[Devices]
WHERE [SerialNumber] = 'SN-APL-IA2')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-APL-IA2', 'iPad Air', 'Apple', 'Tablet', 'iPadOS', '17.2', 'M2', '8GB', 'Used by the design team for sketches.', 'Unavailable');
END

-- 4. Tab S9 Ultra
IF NOT EXISTS (SELECT 1
FROM [dbo].[Devices]
WHERE [SerialNumber] = 'SN-SAM-T9U')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-SAM-T9U', 'Tab S9 Ultra', 'Samsung', 'Tablet', 'Android', '14', 'Snapdragon 8 Gen 2', '16GB', 'Large format tablet for presentations.', 'In Service');
END
GO