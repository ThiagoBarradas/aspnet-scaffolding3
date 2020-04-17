namespace AspNetScaffolding.Extensions.Cache
{
    public class CacheSettings
    {
        public bool Enabled { get; set; }

        public bool UseRedis { get; set; }

        public bool UseLocker { get; set; }

        public string Host { get; set; }

        public int TimeoutInMs { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }

        public string Password { get; set; }

        public int LockerDb { get; set; }

        public string LockerPrefix { get; set; }

        public int LockerTtlInSeconds { get; set; }

        public int CacheDb { get; set; }

        public string CachePrefix { get; set; }

        public int CacheTtlInSeconds { get; set; }

        public string GetCacheConnectionString()
        {
            return $"{this.Host}:{this.Port},ssl={this.Ssl.ToString().ToLower()},password={this.Password},defaultDatabase={this.CacheDb},syncTimeout={this.TimeoutInMs},connectTimeout={this.TimeoutInMs},abortConnect=false";
        }
    }
}
