using DataAccessLibrary.Data;
using DataAccessLibrary.Databases;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelAppWPF
{
    /// <summary>
    /// Interaction logic for Bookings.xaml
    /// </summary>
    public partial class Bookings : Window
    {
        private readonly IDatabaseData _db;
        public Bookings(IDatabaseData db)
        {
            InitializeComponent();
            _db = db;
        }

        public void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            var booking = (FullBookingModel)BookingsDataGrid.SelectedItem;
            if (booking != null)
            {
                _db.CheckInGuest(booking.Id);
                ((Button)sender).Visibility = Visibility.Hidden;
                MessageBox.Show($"{booking.FirstName} {booking.LastName} has been checked in.");
            }
        }

        public void PopulateDataGrid(List<FullBookingModel> bookings)
        {
            BookingsDataGrid.ItemsSource = bookings;
        }
    }
}
