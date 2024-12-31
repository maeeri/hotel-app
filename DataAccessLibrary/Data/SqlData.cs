using DataAccessLibrary.Databases;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";
        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public SqlData()
        {
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            string sql = "dbo.spRoomTypes_GetAvailableTypes";
            return _db.LoadData<RoomTypeModel, dynamic>(sql,
                                new { startDate = startDate, endDate = endDate },
                                connectionStringName,
                                true);
        }

        public List<RoomModel> GetRoomsByRoomType(int roomTypeId, DateTime startDate, DateTime endDate)
        {
            string sql = "dbo.spRooms_GetAvailableRooms";
            return _db.LoadData<RoomModel, dynamic>(sql,
                                new { roomTypeId, startDate, endDate },
                                connectionStringName,
                                true);
        }

        public GuestModel SaveGuest(string firstName, string lastName)
        {
            string sql = "spGuest_Insert";
            GuestModel? output = _db.LoadData<GuestModel, dynamic>(sql,
                        new { FirstName = firstName, LastName = lastName },
                        connectionStringName,
                        true).FirstOrDefault();
            return output ?? new();
        }

        public void CreateBooking(DateTime startDate, DateTime endDate, int roomId, int guestId, decimal totalPrice)
        {
            string sql = "spCreateBooking";
            _db.SaveData<BookingModel, dynamic>(sql,
                         new { startDate, endDate, roomId, guestId, totalPrice },
                         connectionStringName,
                         true);
        }
        public RoomTypeModel GetRoomType(int roomTypeId)
        {
            string sql = "SELECT * FROM dbo.RoomTypes WHERE Id = @Id";
            RoomTypeModel? output = _db.LoadData<RoomTypeModel, dynamic>(sql,
                                new { Id = roomTypeId },
                                connectionStringName,
                                false).FirstOrDefault();
            return output ?? new();
        }

        public List<FullBookingModel> SearchBookings(string lastName)
        {
            string sql = "spGetGuestInformation";
            List<FullBookingModel> output = _db.LoadData<FullBookingModel, dynamic>(sql,
                                new { lastName, startDate = DateTime.Now.Date },
                                connectionStringName,
                                true);
            return output;
        }

        public void BookGuest(string firstName,
                              string lastName,
                              DateTime startDate,
                              DateTime endDate,
                              int roomTypeId)
        {
            GuestModel guest = SaveGuest(firstName, lastName);
            RoomTypeModel roomType = GetRoomType(roomTypeId);
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);
            List<RoomModel> availableRooms = GetRoomsByRoomType(roomTypeId, startDate, endDate);
            CreateBooking(startDate, endDate, availableRooms[0].Id, guest.Id, roomType.Price * timeStaying.Days);
        }

        public void CheckInGuest(int bookingId)
        {
            string sql = "spBookings_CheckIn";
            _db.SaveData<FullBookingModel, dynamic>(sql,
                         new { bookingId = bookingId },
                         connectionStringName,
                         true);
        }
    }
}
