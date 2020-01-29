using AspNetScaffolding.Extensions.JsonSerializer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.QueryFormatter
{
    public static class PathFormatterSettings
    {
        public static void AddPathFormatter(this MvcOptions mvcOptions, JsonSerializerEnum jsonSerializer)
        {
            PathValueProvider.JsonSerializerMode = jsonSerializer;
            mvcOptions.ValueProviderFactories.Add(new PathValueProviderFactory());
        }
    }

    public class PathValueProvider : RouteValueProvider, IValueProvider
    {
        public static JsonSerializerEnum JsonSerializerMode { get; set; }

        public PathValueProvider(
            BindingSource bindingSource,
            RouteValueDictionary values,
            System.Globalization.CultureInfo culture)
            : base(bindingSource, values, culture)
        {
        }

        public override bool ContainsPrefix(string prefix)
        {
            return base.ContainsPrefix(prefix.GetValueConsideringCurrentCase());
        }

        public override ValueProviderResult GetValue(string key)
        {
            return base.GetValue(key.GetValueConsideringCurrentCase());
        }
    }

    public class PathValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var valueProvider = new PathValueProvider(
                BindingSource.Query,
                context.ActionContext.HttpContext.GetRouteData().Values,
                System.Globalization.CultureInfo.CurrentCulture);

            context.ValueProviders.Add(valueProvider);

            return Task.CompletedTask;
        }
    }
}
