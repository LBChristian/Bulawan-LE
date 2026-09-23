using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace BlogDataLibrary.Database
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly string connectionString;

        public SqlDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<T> LoadData<T, U>(
            string sql,
            U parameters,
            CommandType commandType = CommandType.Text)
        {
            using (IDbConnection connection =
                   new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(
                    sql,
                    parameters,
                    commandType: commandType
                ).ToList();

                return rows;
            }
        }

        public void SaveData<T>(
            string sql,
            T parameters,
            CommandType commandType = CommandType.Text)
        {
            using (IDbConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Execute(
                    sql,
                    parameters,
                    commandType: commandType
                );
            }
        }
    }
}