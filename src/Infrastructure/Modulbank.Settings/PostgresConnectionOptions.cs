namespace Modulbank.Settings
{
    public class PostgresConnectionOptions
    {
        public string DatabaseOwner { get; set; }

        public string Context { get; set; }

        public string UserId { get; set; }
        
        public string Server { get; set; }
        
        public string Password { get; set; }
        
        public bool Pooling { get; set; }

        public string SslMode { get; set; }

        public bool? TrustServerCertificate { get; set; } 

        public string DatabaseName { get; set; }

        public string MaintenanceDatabase { get; set; }
    }
}