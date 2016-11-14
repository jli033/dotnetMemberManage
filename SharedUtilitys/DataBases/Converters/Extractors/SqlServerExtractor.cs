using System;
using System.Data;
using System.Data.SqlClient;
using SharedUtilitys.DataBases.Base;

namespace SharedUtilitys.DataBases.Converters.Extractors
{
    public static class SqlServerExtractor
    {
        public static SqlParameter Extract(SqlParamWrapper parameter)
        {
            if (parameter.ParameterType == typeof(decimal))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.Decimal) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(decimal?))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.Decimal) { Value = _getNullableValue(parameter.Value) };
            }
            if (parameter.ParameterType == typeof(int))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.Int) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(int?))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.Int) { Value = _getNullableValue(parameter.Value) };
            }
            if (parameter.ParameterType == typeof(string))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.NVarChar) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(bool))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.Bit) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(DateTime))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.DateTime) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(DateTime?))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.DateTime) { Value = _getNullableValue(parameter.Value) };
            }
            if (parameter.ParameterType == typeof(double))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.Float) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(byte[]))
            {
                return new SqlParameter('@' + parameter.ParameterName, SqlDbType.VarBinary) { Value = parameter.Value };
            }

            return null;
        }

        private static object _getNullableValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }
    }
}
