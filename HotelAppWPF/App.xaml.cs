using DataAccessLibrary.Data;
using DataAccessLibrary.Databases;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;

namespace HotelAppWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config= builder.Build();
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);

            if (config.GetValue<bool>("UseSqLite") && !string.IsNullOrWhiteSpace(config.GetConnectionString("SQLiteDb")))
            {
                services.AddTransient<IDbConnection>(sp => new SQLiteConnection(config.GetConnectionString("SQLiteDb")));
                services.AddTransient<ISQLiteDataAccess, SQLiteDataAccess>();
                services.AddTransient<IDatabaseData, SQLiteData>();
            }
            else
            {
                services.AddTransient<IDbConnection>(sp => new SqlConnection(config.GetConnectionString("SqlDb")));
                services.AddTransient<ISqlDataAccess, SqlServerDataAccess>();
                services.AddTransient<IDatabaseData, SqlData>();
            }
            services.AddTransient<MainWindow>();
            services.AddTransient<Bookings>();

            serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.SetDatabaseData(serviceProvider.GetRequiredService<IDatabaseData>());
            mainWindow.Show();
        }
    }
}
