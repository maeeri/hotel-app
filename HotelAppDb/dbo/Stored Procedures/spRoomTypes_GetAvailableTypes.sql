CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableTypes]
	@startDate DATE,
	@endDate DATE
AS
BEGIN 
	SET NOCOUNT ON;
	SELECT [Id], [Title], [Description], [Price]
	FROM RoomTypes
	WHERE [Id] NOT IN
	(
		SELECT RoomTypes.Id
		FROM RoomTypes
		INNER JOIN Rooms ON RoomTypes.Id = Rooms.RoomTypeId
		INNER JOIN Bookings ON Rooms.Id = Bookings.RoomId
		WHERE (@startDate BETWEEN Bookings.CheckInDate AND DATEADD(day, -1,Bookings.CheckOutDate)) OR
				(@endDate BETWEEN DATEADD(day, 1, Bookings.CheckInDate) AND Bookings.CheckOutDate)
	)
	GROUP BY [Id], [Title], [Description], [Price]
END
