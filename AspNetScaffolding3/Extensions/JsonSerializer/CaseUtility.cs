using PackUtils;

namespace AspNetScaffolding.Extensions.JsonSerializer
{
    public static class CaseUtility
    {
        public static JsonSerializerEnum JsonSerializerMode { get; set; }

        public static string GetValueConsideringCurrentCase(this string value)
        {
            switch (JsonSerializerMode)
            {
                case JsonSerializerEnum.Camelcase:
                    value = value.ToCamelCase();
                    break;
                case JsonSerializerEnum.Snakecase:
                    value = value.ToSnakeCase();
                    break;
                case JsonSerializerEnum.Lowercase:
                    value = value.ToLowerCase();
                    break;
            }

            return value;
        }
    }
}
