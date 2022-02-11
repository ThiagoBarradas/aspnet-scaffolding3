namespace AspNetScaffolding3.Extensions.RequestLimit
{
    /// <summary>
    /// Was created only to keep retrocompatibility with older verions. This could be the same as the IpRateLimitingAdditional class
    /// </summary>
    public class RateLimitingAdditional
    {
        public bool Enabled { get; set; }

        public bool ByUrlResource { get; set; }

        public string UrlResource { get; set; }
    }
}