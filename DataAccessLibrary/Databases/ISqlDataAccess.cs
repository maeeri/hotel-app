
namespace DataAccessLibrary.Databases
{
    public interface ISqlDataAccess
    {
        List<T> LoadData<T, U>(string sql,
                               U parameters,
                               string connectionStringName,
                               bool isStoredProcedure);
        void SaveData<T, U>(string sql,
                            U parameters,
                            string connectionStringName,
                            bool isStoredProcedure);
    }
}