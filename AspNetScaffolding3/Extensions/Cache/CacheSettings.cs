namespace AspNetScaffolding.Extensions.Cache
{
    public class CacheSettings
    {
        public bool Enabled { get; set; }

        public string RedisConnectionString { get; set; }

        public bool IsDistributed => !string.IsNullOrWhiteSpace(this.RedisConnectionString);
    }
}
