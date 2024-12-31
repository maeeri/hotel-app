CREATE PROCEDURE [dbo].[spRooms_GetAvailableRooms]
	@RoomTypeId int,
	@startDate date,
	@endDate date
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Id],
		[RoomNumber]
	FROM
		[dbo].[Rooms]
	WHERE
		[RoomTypeId] = @RoomTypeId
		AND [Id] NOT IN (
			SELECT
				[RoomId]
			FROM
				[dbo].[Bookings]
			WHERE (@startDate BETWEEN Bookings.CheckInDate AND DATEADD(day, -1,Bookings.CheckOutDate)) OR
				(@endDate BETWEEN DATEADD(day, 1, Bookings.CheckInDate) AND Bookings.CheckOutDate) OR
				(@startDate < Bookings.CheckInDate AND @endDate > Bookings.CheckOutDate)
		)
END
RETURN 0
