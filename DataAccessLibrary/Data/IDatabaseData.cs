using DataAccessLibrary.Models;

namespace DataAccessLibrary.Data
{
    public interface IDatabaseData
    {
        void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId);
        void CheckInGuest(int bookingId);
        void CreateBooking(DateTime startDate, DateTime endDate, int roomId, int guestId, decimal totalPrice);
        List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate);
        List<RoomModel> GetRoomsByRoomType(int roomTypeId, DateTime startDate, DateTime endDate);
        RoomTypeModel GetRoomType(int roomTypeId);
        GuestModel SaveGuest(string firstName, string lastName);
        List<FullBookingModel> SearchBookings(string lastName);
    }
}