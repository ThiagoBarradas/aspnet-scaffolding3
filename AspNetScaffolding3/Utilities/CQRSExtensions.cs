using AspNetScaffolding.Extensions.JsonSerializer;
using System.Linq;

namespace AspNetScaffolding.Utilities
{
    public static class CQRSExtensions
    {
        public static string PropertyNameResolver(string propertyName)
        {
            if (propertyName == null)
            {
                return null;
            }

            var parts = propertyName.Split(".").ToList();
            return string.Join(".", parts.Select(i => i.GetValueConsideringCurrentCase()));
        }
    }
}
