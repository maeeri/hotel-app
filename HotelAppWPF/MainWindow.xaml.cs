using DataAccessLibrary.Data;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDatabaseData _db;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetDatabaseData(IDatabaseData db)
        {
            _db = db;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text;
            var result = _db.SearchBookings(query);
            var bookings = App.serviceProvider.GetRequiredService<Bookings>();
            bookings.PopulateDataGrid(result);
            bookings.Show();
        }
    }
}