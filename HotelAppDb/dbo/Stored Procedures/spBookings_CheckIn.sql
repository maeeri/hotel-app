CREATE PROCEDURE [dbo].[spBookings_CheckIn]
	@bookingId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Bookings]
	SET CheckedIn = 1
	WHERE Id = @bookingId;

END
RETURN 0
