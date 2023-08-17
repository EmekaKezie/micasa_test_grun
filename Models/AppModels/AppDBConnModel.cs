using DbFactory;

namespace TheEstate.Models.AppModels
{
    public class AppDBConnModel
    {
        public string ProviderName { get; set; }
        public string DatabaseType { get; set; }
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int MaxPoolSize { get; set; }
        public int ConnectionTimeout { get; set; }
        public int CommandTimeout { get; set; }

        public static implicit operator DbConnectionParam(AppDBConnModel v)
        {
            throw new NotImplementedException();
        }
    }
}
