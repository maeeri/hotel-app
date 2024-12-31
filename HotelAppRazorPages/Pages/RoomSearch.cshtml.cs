using DataAccessLibrary.Data;
using DataAccessLibrary.Databases;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelAppRazorPages.Pages
{
    public class RoomSearchModel : PageModel
    {
        private readonly IDatabaseData _db;
        public List<RoomTypeModel> AvailableRoomTypes = new List<RoomTypeModel>();
        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } 
        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [BindProperty]
        public decimal Price { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool SearchEnabled { get; set; } = false;

        public void OnGet(DateTime startDate, DateTime endDate)
        {
            if (SearchEnabled == true)
            {
                this.StartDate = startDate;
                this.EndDate = endDate;
                this.AvailableRoomTypes = _db.GetAvailableRoomTypes(StartDate, EndDate);
            }
            else
            {
                this.StartDate = DateTime.Today;
                this.EndDate = DateTime.Today.AddDays(1);
            }
        }

        public RoomSearchModel(IDatabaseData db)
        {
            _db = db;
        }

        public IActionResult OnPost()
        {
            return RedirectToPage(new {SearchEnabled = true, 
                StartDate = StartDate.ToString("yyyy-MM-dd"), 
                EndDate = EndDate.ToString("yyyy-MM-dd")
            });
        }

    }
}
