CREATE PROCEDURE [dbo].[spGetGuestInformation]
	@lastName NVARCHAR(50),
	@startDate DATE
AS
BEGIN
	SET NOCOUNT ON;

	SELECT b.[Id], g.[FirstName], g.[LastName], b.[CheckInDate], b.[CheckOutDate], r.[RoomNumber], r.[RoomTypeId], b.[TotalPrice], b.[CheckedIn]
	FROM [dbo].[Bookings] b
	JOIN [dbo].[Rooms] r ON b.[RoomId] = r.[Id]
	JOIN [dbo].[Guests] g ON b.[GuestId] = g.[Id]
	WHERE g.[LastName] = @lastName
	AND b.[CheckInDate] = @startDate;
END
