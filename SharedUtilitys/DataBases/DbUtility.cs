using System;
using System.Collections.Generic;
using System.Data;
using SharedUtilitys.DataBases.Base;
using SharedUtilitys.DataBases.Configs;
using SharedUtilitys.DataBases.Converters;

namespace SharedUtilitys.DataBases
{
    public class DbUtility : IDisposable
    {
        private readonly IDbConnection _sqlConnection;
        private IDbTransaction _sqlTransaction;

        public List<SqlParamWrapper> SqlParameters { get; set; }
        
        public static DbUtility GetInstance(string connectionString)
        {
            return new DbUtility(connectionString);
        }

        public static DbUtility GetInstance()
        {
            return new DbUtility(DatabaseConfig.ConnectionString);
        }

        public static DbUtility GetInstanceByNoConnect()
        {
            return new DbUtility();
        }

        private DbUtility()
        {
        }

        private DbUtility(string connectionString)
        {
            _sqlConnection = SqlObject.GetInstance(connectionString).Connection;
            SqlParameters = new List<SqlParamWrapper>();
        }

        public void Dispose()
        {
            if (_sqlTransaction != null)
            {
                _sqlTransaction.Rollback();
                _sqlTransaction.Dispose();
                _sqlTransaction = null;
            }

            if (_sqlConnection != null)
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }
        }

        public bool ConnectionTest(string connectionString)
        {
            IDbConnection conn = null;
            bool result;

            try
            {
                conn = SqlObject.GetInstance(connectionString).Connection;
                conn.Close();
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return result;
        }

        public void BeginTransaction()
        {
            _sqlTransaction = _sqlConnection.BeginTransaction();
        }

        public void Rollback()
        {
            _sqlTransaction.Rollback();
            _sqlTransaction.Dispose();
            _sqlTransaction = null;
        }

        public void Commit()
        {
            _sqlTransaction.Commit();
            _sqlTransaction.Dispose();
            _sqlTransaction = null;
        }

        public int ExecuteNonQuery(string sql)
        {
            var command = _sqlConnection.CreateCommand();

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }
            command.CommandTimeout = 60 * 1000 * 60;
            
            _setSqlParameters(command);
 
            command.CommandText = sql;
            var effRows=command.ExecuteNonQuery();
            command.Dispose();

            SqlParameters = new List<SqlParamWrapper>();
            return effRows;
        }

        public IEnumerable<Dictionary<string, object>> ExecuteReader(string sql)
        {
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }

            var reader = command.ExecuteReader();
            command.Dispose();

            var result = DataReaderConverter.GetList(reader);
            reader.Close();

            SqlParameters = new List<SqlParamWrapper>();

            return result;
        }

        public DataSet GetDataSet(string sql)
        {
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }

            var adapter= SqlObject.CreateAdapter(command);
            DataSet dtSet= new DataSet();
            adapter.Fill(dtSet);                      
            SqlParameters = new List<SqlParamWrapper>();
            return dtSet;
        }

        public void ExecuteReaderModel(string sql,Object model){
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }

            var reader = command.ExecuteReader();
            command.Dispose();

            DataReaderConverter.ConvertModel(reader, model);
            reader.Close();
            SqlParameters = new List<SqlParamWrapper>();
        }

        public void ExecuteReaderModelList(string sql, Object list)
        {
            var command = _sqlConnection.CreateCommand();
            command.CommandText = sql;

            _setSqlParameters(command);

            if (_sqlTransaction != null)
            {
                command.Transaction = _sqlTransaction;
            }

            var reader = command.ExecuteReader();
            command.Dispose();

            DataReaderConverter.ConvertModelList(reader, list);
            reader.Close();
            SqlParameters = new List<SqlParamWrapper>();
        }



        public void AddParameter(string parameterName, object value)
        {
            SqlParameters.Add(new SqlParamWrapper(parameterName, value));   
        }

        private void _setSqlParameters(IDbCommand command)
        {
            command.Parameters.Clear();

            if (SqlParameters != null)
            {
                foreach (var item in SqlParameters)
                {
                    command.Parameters.Add(DbParamConverter.Convert(item));
                }
            }
        }
    }
}
