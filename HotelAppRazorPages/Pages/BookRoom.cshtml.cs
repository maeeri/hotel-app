using DataAccessLibrary.Data;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelAppRazorPages.Pages
{
    public class BookRoomModel : PageModel
    {
        IDatabaseData _db;
        public BookRoomModel(IDatabaseData db)
        {
            _db = db;
        }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public int RoomTypeId { get; set; }

        [BindProperty]
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [BindProperty]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [BindProperty]
        [DataType(DataType.Text)]
        public string LastName { get; set; }


        public void OnGet(int id, DateTime start, DateTime end)
        {
            var routeData = RouteData.Values;
            var roomType = _db.GetRoomType(id);
            Title = roomType.Title;
            Description = roomType.Description;
            StartDate = start;
            EndDate = end;
            RoomTypeId = id;
        }

        public IActionResult OnPost(DateTime start, DateTime end)
        {
            _db.BookGuest(FirstName, LastName, start, end, RoomTypeId);
            return RedirectToPage("Index");
        }
    }
}
