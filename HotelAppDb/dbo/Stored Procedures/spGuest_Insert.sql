CREATE PROCEDURE [dbo].[spGuest_Insert]
	@firstName varchar(50),
	@lastName varchar(50)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Guests] WHERE [FirstName] = @firstName AND [LastName] = @lastName)
	BEGIN
		INSERT INTO [dbo].[Guests] ([FirstName], [LastName])
		VALUES (@firstName, @lastName);
	END

	SELECT TOP 1 [Id], [FirstName], [LastName] FROM [dbo].[Guests] WHERE [FirstName] = @firstName AND [LastName] = @lastName;
END

