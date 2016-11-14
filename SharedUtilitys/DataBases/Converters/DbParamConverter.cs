using System.Data;
using SharedUtilitys.DataBases.Base;
using SharedUtilitys.DataBases.Configs;
using SharedUtilitys.DataBases.Converters.Extractors;

namespace SharedUtilitys.DataBases.Converters
{
    public static class DbParamConverter
    {
        public static IDbDataParameter Convert(SqlParamWrapper parameter)
        {
            switch (DatabaseConfig.DatabaseType)
            {
                case DatabaseEnum.SqlServer:
                    return SqlServerExtractor.Extract(parameter);  

                case DatabaseEnum.MySql:
                    return MySqlExtractor.Extract(parameter);  
            }

            return null;
        }
    }
}
