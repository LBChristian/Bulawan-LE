using System.Data;

namespace BlogDataLibrary.Database
{
    public interface ISqlDataAccess
    {
        List<T> LoadData<T, U>(
            string sql,
            U parameters,
            CommandType commandType = CommandType.Text);

        void SaveData<T>(
            string sql,
            T parameters,
            CommandType commandType = CommandType.Text);
    }
}