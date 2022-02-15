using PackUtils;

namespace AspNetScaffolding.Extensions.JsonSerializer
{
    public static class CaseUtility
    {
        public static JsonSerializerEnum JsonSerializerMode { get; set; }

        public static string GetValueConsideringCurrentCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.ToCase(JsonSerializerMode.ToString());
        }
    }
}
