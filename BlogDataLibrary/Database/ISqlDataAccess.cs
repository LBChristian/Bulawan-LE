namespace BlogDataLibrary.Database
{
    public interface ISqlDataAccess
    {
        List<T> LoadData<T, U>(string sql, U parameters);

        void SaveData<T>(string sql, T parameters);
    }
}