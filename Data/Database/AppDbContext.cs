using DbFactory;
using Serilog;
using TheEstate.Models.AppModels;

namespace TheEstate.Data.Database
{
    public class AppDbContext
    {
        private static DbConnectionParam GetConstr()
        {
            DbConnectionParam? conn = null;

            try
            {
                AppDBConnModel param = Utility.ConvertFileContentToJson<AppDBConnModel>("config", "dbconn.json");
                if (param != null)
                {
                    conn = new DbConnectionParam
                    {
                        Server = param.Server,
                        ProviderName = param.ProviderName,
                        DatabaseName = param.DatabaseName,
                        UserId = param.UserId,
                        Password = param.Password,
                        Port = param.Port,
                        DatabaseType = param.DatabaseType,
                        CommandTimeout = param.CommandTimeout,
                        ConnectionTimeout = param.ConnectionTimeout,
                        MaxPoolSize = param.MaxPoolSize,
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[DBConn] -> {ex.Message}");
                throw;
            }
            return conn!;
        }

        public static DbController db = new(GetConstr());

    }
}
