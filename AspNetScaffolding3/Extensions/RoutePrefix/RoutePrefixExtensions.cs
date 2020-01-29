using Microsoft.AspNetCore.Mvc;

namespace AspNetScaffolding.Extensions.RoutePrefix
{
    public static class RoutePrefixExtensions
    {
        public static void UseCentralRoutePrefix(this MvcOptions mvcOptions, string pathPrefix)
        {
            var routeAttribute = new RouteAttribute(pathPrefix ?? "");
            mvcOptions.Conventions.Insert(0, new RoutePrefixConvention(routeAttribute));
        }
    }
}
