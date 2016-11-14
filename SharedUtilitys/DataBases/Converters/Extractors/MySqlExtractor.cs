using System;
using MySql.Data.MySqlClient;
using SharedUtilitys.DataBases.Base;

namespace SharedUtilitys.DataBases.Converters.Extractors
{
    public class MySqlExtractor
    {
        public static MySqlParameter Extract(SqlParamWrapper parameter)
        {
            if (parameter.ParameterType == typeof(decimal))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.Decimal) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(decimal?))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.Decimal) { Value = _getNullableValue(parameter.Value) };
            }
            if (parameter.ParameterType == typeof(int))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.Int32) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(int?))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.Int32) { Value = _getNullableValue(parameter.Value) };
            }
            if (parameter.ParameterType == typeof(string))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.String) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(bool))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.Bit) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(DateTime))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.DateTime) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(DateTime?))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.DateTime) { Value = _getNullableValue(parameter.Value) };
            }
            if (parameter.ParameterType == typeof(double))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.Float) { Value = parameter.Value };
            }
            if (parameter.ParameterType == typeof(byte[]))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.VarBinary) { Value = parameter.Value };
            }
            if (parameter.Value ==null)
            {
                return new MySqlParameter('?' + parameter.ParameterName, DBNull.Value);
            }
            if (parameter.ParameterType == typeof(long))
            {
                return new MySqlParameter('?' + parameter.ParameterName, MySqlDbType.Int64) { Value = _getNullableValue(parameter.Value) };
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
