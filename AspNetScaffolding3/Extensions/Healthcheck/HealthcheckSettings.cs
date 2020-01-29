namespace AspNetScaffolding.Extensions.Healthcheck
{
    public class HealthcheckSettings
    {
        public bool Enabled { get; set; }

        public string Path { get; set; }

        public bool LogEnabled { get; set; }
    }
}
