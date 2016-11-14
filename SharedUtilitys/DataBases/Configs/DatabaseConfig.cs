using System;
using System.Configuration;

namespace SharedUtilitys.DataBases.Configs
{
    public static class DatabaseConfig
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }

        public static DatabaseEnum DatabaseType
        {
            get
            {
                switch (ConfigurationManager.AppSettings["DatabaseType"])
                {
                    case "SqlServer":
                        return DatabaseEnum.SqlServer;
                    case "MySql":
                        return DatabaseEnum.MySql;
                    default:
                        throw new Exception("アプリケーション構成ファイルのAppSettings.DatabaseTypeに誤りがあります。");
                }
            }
        }
    }
}
