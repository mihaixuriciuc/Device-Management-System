USE DeviceDb;
GO

-- Ensure we are only inserting if the table is empty or specific records don't exist
-- We use the Name and Manufacturer as a unique check for this dummy data

-- 1. Add a Phone
IF NOT EXISTS (SELECT 1
FROM [dbo].[Devices]
WHERE [Name] = 'iPhone 15 Pro' AND [Manufacturer] = 'Apple')
BEGIN
    INSERT INTO [dbo].[Devices]
        ([Name], [Manufacturer], [Type], [Operating System], [OS version], [Processor], [RAM amount], [Description], [Status])
    VALUES
        ('iPhone 15 Pro', 'Apple', 'Phone', 'iOS', '17.4', 'A17 Pro', '8GB', 'Main corporate handheld device.', 'Available');
END

-- 2. Add another Phone
IF NOT EXISTS (SELECT 1
FROM [dbo].[Devices]
WHERE [Name] = 'Galaxy S24' AND [Manufacturer] = 'Samsung')
BEGIN
    INSERT INTO [dbo].[Devices]
        ([Name], [Manufacturer], [Type], [Operating System], [OS version], [Processor], [RAM amount], [Description], [Status])
    VALUES
        ('Galaxy S24', 'Samsung', 'Phone', 'Android', '14', 'Snapdragon 8 Gen 3', '12GB', 'Standard Android option for developers.', 'Available');
END

-- 3. Add a Tablet
IF NOT EXISTS (SELECT 1
FROM [dbo].[Devices]
WHERE [Name] = 'iPad Air' AND [Manufacturer] = 'Apple')
BEGIN
    INSERT INTO [dbo].[Devices]
        ([Name], [Manufacturer], [Type], [Operating System], [OS version], [Processor], [RAM amount], [Description], [Status])
    VALUES
        ('iPad Air', 'Apple', 'Tablet', 'iPadOS', '17.2', 'M2', '8GB', 'Used by the design team for sketches.', 'Unavailable');
END

-- 4. Add another Tablet
IF NOT EXISTS (SELECT 1
FROM [dbo].[Devices]
WHERE [Name] = 'Tab S9 Ultra' AND [Manufacturer] = 'Samsung')
BEGIN
    INSERT INTO [dbo].[Devices]
        ([Name], [Manufacturer], [Type], [Operating System], [OS version], [Processor], [RAM amount], [Description], [Status])
    VALUES
        ('Tab S9 Ultra', 'Samsung', 'Tablet', 'Android', '14', 'Snapdragon 8 Gen 2', '16GB', 'Large format tablet for presentations.', 'In Service');
END
GO