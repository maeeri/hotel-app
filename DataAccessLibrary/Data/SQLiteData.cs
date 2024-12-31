using DataAccessLibrary.Databases;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Data
{
    public class SQLiteData : IDatabaseData
    {
        private readonly ISQLiteDataAccess _db;
        private const string connectionStringName = "SQLiteDb";

        public SQLiteData(ISQLiteDataAccess db)
        {
            _db = db;
        }
        public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
            GuestModel guest = GetGuest(firstName, lastName);
            if (guest == null)
            {
                guest = SaveGuest(firstName, lastName);
            }

            RoomTypeModel roomType = GetRoomType(roomTypeId);
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);
            List<RoomModel> availableRooms = GetRoomsByRoomType(roomTypeId, startDate, endDate);
            CreateBooking(startDate, endDate, availableRooms[0].Id, guest.Id, roomType.Price * timeStaying.Days);
        }

        public void CheckInGuest(int bookingId)
        {
            string sql = @"UPDATE Bookings
	                    SET CheckedIn = 1
	                    WHERE Id = @bookingId;";
            _db.SaveData<FullBookingModel, dynamic>(sql,
                         new { bookingId = bookingId },
                         connectionStringName);
        }

        public void CreateBooking(DateTime startDate, DateTime endDate, int roomId, int guestId, decimal totalPrice)
        {
            string sql = @"INSERT INTO Bookings (CheckInDate, CheckOutDate, RoomId, GuestId, TotalPrice)
	                        VALUES (@startDate, @endDate, @roomId, @guestId, @totalPrice);";
            _db.SaveData<BookingModel, dynamic>(sql, new { startDate, endDate, roomId, guestId, totalPrice }, connectionStringName);
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            string sql = @"SELECT Id, Title, Description, Price
                        FROM RoomTypes
                        WHERE Id NOT IN (
                            SELECT RoomTypes.Id
                            FROM RoomTypes
                            INNER JOIN Rooms ON RoomTypes.Id = Rooms.RoomTypeId
                            INNER JOIN Bookings ON Rooms.Id = Bookings.RoomId
                            WHERE (@startDate BETWEEN Bookings.CheckInDate AND DATE(Bookings.CheckOutDate, '-1 day'))
                            OR (@endDate BETWEEN DATE(Bookings.CheckInDate, '+1 day') AND Bookings.CheckOutDate)
                        )
                        GROUP BY Id, Title, Description, Price;";
            List<RoomTypeModel> roomtypes = _db.LoadData<RoomTypeModel, dynamic>(sql, new { startDate = startDate, endDate = endDate }, connectionStringName);
            return roomtypes.Select(rt => new RoomTypeModel() { Id = rt.Id, Title = rt.Title, Description = rt.Description, Price = rt.Price / 100 }).ToList();
        }

        public List<RoomModel> GetRoomsByRoomType(int roomTypeId, DateTime startDate, DateTime endDate)
        {
            string sql = @"SELECT Id, RoomNumber
                         FROM Rooms
                         WHERE RoomTypeId = @RoomTypeId
                         AND Id NOT IN (SELECT RoomId
                         FROM Bookings
                         WHERE (@startDate BETWEEN Bookings.CheckInDate AND DATE(Bookings.CheckOutDate, '-1 day')) OR
                         (@endDate BETWEEN DATE(Bookings.CheckInDate, '+1 day') AND Bookings.CheckOutDate) OR
                         (@startDate < Bookings.CheckInDate AND @endDate > Bookings.CheckOutDate))";
            return _db.LoadData<RoomModel, dynamic>(sql, new { RoomTypeId = roomTypeId, startDate = startDate, endDate = endDate }, connectionStringName);
        }

        public RoomTypeModel GetRoomType(int roomTypeId)
        {
            string sql = "SELECT * FROM RoomTypes WHERE Id = @Id";
            RoomTypeModel? output = _db.LoadData<RoomTypeModel, dynamic>(sql,
                                new { Id = roomTypeId },
                                connectionStringName).FirstOrDefault();
            return output ?? new();
        }

        public GuestModel SaveGuest(string firstName, string lastName)
        {
            string sql = @"INSERT INTO Guests (FirstName, LastName)
                            VALUES (@firstName, @lastName);";
            _db.SaveData<GuestModel, dynamic>(sql, new { firstName = firstName, lastName = lastName }, connectionStringName);
            return GetGuest(firstName, lastName);
        }

        public GuestModel GetGuest(string firstName, string lastName)
        {
            string sql = @"SELECT Id, FirstName, LastName FROM Guests WHERE FirstName = @firstName AND LastName = @lastName";
            return _db.LoadData<GuestModel, dynamic>(sql, new { firstName = firstName, lastName = lastName }, connectionStringName).FirstOrDefault();
        }

        public List<FullBookingModel> SearchBookings(string lastName)
        {
            string sql = @"SELECT b.Id, g.FirstName, g.LastName, b.CheckInDate, b.CheckOutDate, r.RoomNumber, r.RoomTypeId, b.TotalPrice, b.CheckedIn
	                        FROM Bookings b
	                        JOIN Rooms r ON b.RoomId = r.Id
	                        JOIN Guests g ON b.GuestId = g.Id
	                        WHERE g.LastName = @lastName
	                        AND b.CheckInDate = @startDate;";

            var output = _db.LoadData<FullBookingModel, dynamic>(sql, new { lastName, startDate = DateTime.Today }, connectionStringName);
            output.ForEach(fb => fb.TotalPrice /= 100);
            return output;
        }

        public List<RoomModel> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            List<int> roomTypeIds = GetAvailableRoomTypes(startDate, endDate).Select(rt => rt.Id).ToList();
            List<RoomModel> output = new();
            foreach (int roomTypeId in roomTypeIds)
            {
                output.AddRange(GetRoomsByRoomType(roomTypeId, startDate, endDate));
            }
            return output;
        }
    }
}
