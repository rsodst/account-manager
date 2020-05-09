namespace Modulbank.Settings
{
    public class PostgresConnectionOptions
    {
        public string UserId { get; set; }
        
        public string Server { get; set; }
        
        public string Password { get; set; }
        
        public bool Pooling { get; set; }
    }
}