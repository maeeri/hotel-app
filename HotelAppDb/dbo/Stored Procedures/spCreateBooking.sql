CREATE PROCEDURE [dbo].[spCreateBooking]
	@startDate DATE, 
	@endDate DATE, 
	@roomId INT, 
	@guestId INT,
	@totalPrice DECIMAL(18,2)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Bookings] (CheckInDate, CheckOutDate, RoomId, GuestId, TotalPrice)
	VALUES (@startDate, @endDate, @roomId, @guestId, @totalPrice);

END
