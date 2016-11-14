using System;

namespace SharedUtilitys.DataBases.Base
{
    public class SqlParamWrapper
    {
        public string ParameterName { get; set; }
        public Type ParameterType { get; set; }
        // TODO: SqlParamWrapper IsNullable
        //bool IsNullable { get; set; }
        public object Value { get; set; }

        public SqlParamWrapper(string parameterName, object value)
        {
            ParameterName = parameterName;

            if (value != null)
            {
                ParameterType = value.GetType();
            }

            Value = value;
        }
    }
}
