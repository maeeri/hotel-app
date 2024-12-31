
namespace DataAccessLibrary.Databases
{
    public interface ISQLiteDataAccess
    {
        List<T> LoadData<T, U>(string sql, U parameters, string connectionStringName);
        void SaveData<T, U>(string sql, U parameters, string connectionStringName);
    }
}