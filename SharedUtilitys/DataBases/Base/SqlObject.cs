using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using SharedUtilitys.DataBases.Configs;

namespace SharedUtilitys.DataBases.Base
{
    public class SqlObject
    {
        public IDbConnection Connection { get; set; }

        public static SqlObject GetInstance(string connectionString)
        {
            return new SqlObject(connectionString);
        }

        private SqlObject(string connectionString)
        {
            switch (DatabaseConfig.DatabaseType)
            {
                case DatabaseEnum.SqlServer:
                    Connection = new SqlConnection();
                    break;
                case DatabaseEnum.MySql:
                    Connection = new MySqlConnection();
                    break;
            }

            Connection.ConnectionString = connectionString;
            Connection.Open();
        }

        public static IDbDataAdapter CreateAdapter(IDbCommand command)
        {
            IDbDataAdapter adapter = null;
            switch (DatabaseConfig.DatabaseType)
            {
                case DatabaseEnum.SqlServer:
                    adapter = new SqlDataAdapter((SqlCommand)command);
                    break;
                case DatabaseEnum.MySql:
                    adapter = new MySqlDataAdapter((MySqlCommand)command);
                    break;
                default:
                    break;
            }
            return adapter;
        }
    }
}
