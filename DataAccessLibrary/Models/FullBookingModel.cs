using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class FullBookingModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public decimal TotalPrice { get; set; }
        public bool CheckedIn { get; set; }
    }
}

