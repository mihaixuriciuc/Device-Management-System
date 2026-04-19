USE DeviceDb;
GO

-- ====================== SEED DATA - 15 DEVICES ======================

-- 1. iPhone 15 Pro
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-APL-P15')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-APL-P15', 'iPhone 15 Pro', 'Apple', 'Phone', 'iOS', '17.4', 'A17 Pro', '8GB', 'Main corporate handheld device.', 'Available');
END

-- 2. Galaxy S24 Ultra
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-SAM-S24U')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-SAM-S24U', 'Galaxy S24 Ultra', 'Samsung', 'Phone', 'Android', '14', 'Snapdragon 8 Gen 3', '12GB', 'Premium Android flagship.', 'Available');
END

-- 3. iPad Air 5
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-APL-IA5')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-APL-IA5', 'iPad Air 5', 'Apple', 'Tablet', 'iPadOS', '17.2', 'M1', '8GB', 'Used by the design team for sketches and prototypes.', 'Unavailable');
END

-- 4. Galaxy Tab S9 Ultra
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-SAM-T9U')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-SAM-T9U', 'Galaxy Tab S9 Ultra', 'Samsung', 'Tablet', 'Android', '14', 'Snapdragon 8 Gen 2', '16GB', 'Large format tablet for presentations and meetings.', 'In Service');
END

-- 5. iPhone 14
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-APL-P14')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-APL-P14', 'iPhone 14', 'Apple', 'Phone', 'iOS', '16.6', 'A15 Bionic', '6GB', 'Standard issue for sales team.', 'Available');
END

-- 6. Pixel 8 Pro
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-GOO-P8P')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-GOO-P8P', 'Pixel 8 Pro', 'Google', 'Phone', 'Android', '14', 'Tensor G3', '12GB', 'Developer device for testing.', 'Available');
END

-- 7. iPad Pro 12.9"
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-APL-IP12')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-APL-IP12', 'iPad Pro 12.9', 'Apple', 'Tablet', 'iPadOS', '17.3', 'M2', '16GB', 'High-performance tablet for creative team.', 'In Use');
END

-- 8. Galaxy Tab S8
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-SAM-TS8')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-SAM-TS8', 'Galaxy Tab S8', 'Samsung', 'Tablet', 'Android', '13', 'Snapdragon 8 Gen 1', '8GB', 'General purpose tablet.', 'Available');
END

-- 9. iPhone SE (2022)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-APL-SE22')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-APL-SE22', 'iPhone SE 2022', 'Apple', 'Phone', 'iOS', '16.5', 'A15 Bionic', '4GB', 'Budget option for field staff.', 'Available');
END

-- 10. OnePlus 11
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-ONE-11')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-ONE-11', 'OnePlus 11', 'OnePlus', 'Phone', 'Android', '13', 'Snapdragon 8 Gen 2', '16GB', 'High-performance Android phone.', 'Unavailable');
END

-- 11. Surface Pro 9
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-MIC-SP9')   -- Note: Type is still Tablet to fit your schema
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-MIC-SP9', 'Surface Pro 9', 'Microsoft', 'Tablet', 'Windows', '11', 'Intel i7', '16GB', 'Hybrid tablet/laptop for managers.', 'In Service');
END

-- 12. Galaxy Z Fold5
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-SAM-ZF5')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-SAM-ZF5', 'Galaxy Z Fold5', 'Samsung', 'Phone', 'Android', '13', 'Snapdragon 8 Gen 2', '12GB', 'Foldable device for executives.', 'Available');
END

-- 13. iPad Mini 6
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-APL-IM6')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-APL-IM6', 'iPad Mini 6', 'Apple', 'Tablet', 'iPadOS', '16.6', 'A15 Bionic', '4GB', 'Compact tablet for reading and notes.', 'Available');
END

-- 14. Redmi Note 12 Pro
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-RED-N12')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-RED-N12', 'Redmi Note 12 Pro', 'Xiaomi', 'Phone', 'Android', '13', 'Dimensity 1080', '8GB', 'Affordable high-spec phone.', 'In Use');
END

-- 15. Lenovo Tab P11 Pro
IF NOT EXISTS (SELECT 1 FROM [dbo].[Devices] WHERE [SerialNumber] = 'SN-LEN-P11')
BEGIN
    INSERT INTO [dbo].[Devices]
        (SerialNumber, Name, Manufacturer, [Type], OperatingSystem, OsVersion, Processor, RamAmount, [Description], [Status])
    VALUES
        ('SN-LEN-P11', 'Lenovo Tab P11 Pro', 'Lenovo', 'Tablet', 'Android', '12', 'Snapdragon 870', '8GB', 'Productivity tablet for marketing team.', 'Available');
END

GO

PRINT '✅ Seed data inserted successfully (15 devices).';