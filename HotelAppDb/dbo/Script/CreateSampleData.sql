/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
IF NOT EXISTS(SELECT 1 FROM dbo.RoomTypes)
-- Insert RoomTypes
BEGIN
	INSERT INTO RoomTypes (Title, Description, Price)
	VALUES ('Single', 'A room with a single bed', 50.00),
			('Double', 'A room with two beds', 75.00),
			('Suite', 'A room with a king size bed and a living room', 150.00);
END

IF NOT EXISTS(SELECT 1 FROM dbo.Rooms)
-- Insert Rooms
BEGIN
	DECLARE @RoomTypeId INT
	SELECT @RoomTypeId = Id FROM RoomTypes WHERE Title = 'Single'
	INSERT INTO Rooms (RoomNumber, RoomTypeId)
	VALUES	('101', @RoomTypeId),
			('102', @RoomTypeId),
			('103', @RoomTypeId),
			('104', @RoomTypeId),
			('105', @RoomTypeId);
	SELECT @RoomTypeId = Id FROM RoomTypes WHERE Title = 'Double'
	INSERT INTO Rooms (RoomNumber, RoomTypeId)
	VALUES	('201', @RoomTypeId),
			('202', @RoomTypeId),
			('203', @RoomTypeId),
			('204', @RoomTypeId),
			('205', @RoomTypeId);
	SELECT @RoomTypeId = Id FROM RoomTypes WHERE Title = 'Suite'
	INSERT INTO Rooms (RoomNumber, RoomTypeId)
	VALUES	('301', @RoomTypeId),
			('302', @RoomTypeId),
			('303', @RoomTypeId),
			('304', @RoomTypeId),
			('305', @RoomTypeId);
END