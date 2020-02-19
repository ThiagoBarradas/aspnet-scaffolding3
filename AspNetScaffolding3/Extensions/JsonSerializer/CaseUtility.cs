using PackUtils;

namespace AspNetScaffolding.Extensions.JsonSerializer
{
    public static class CaseUtility
    {
        public static JsonSerializerEnum JsonSerializerMode { get; set; }

        public static string GetValueConsideringCurrentCase(this string value)
        {
            return value.ToCase(JsonSerializerMode.ToString());
        }
    }
}
