using AspNetScaffolding.Extensions.JsonSerializer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.QueryFormatter
{
    public static class QueryFormatterSettings
    {
        public static void AddQueryFormatter(this MvcOptions mvcOptions, JsonSerializerEnum jsonSerializer)
        {
            CaseQueryValueProvider.JsonSerializerMode = jsonSerializer;
            mvcOptions.ValueProviderFactories.Add(new CaseQueryValueProviderFactory());
        }
    }

    public class CaseQueryValueProvider : QueryStringValueProvider, IValueProvider
    {
        public static JsonSerializerEnum JsonSerializerMode { get; set; }

        public CaseQueryValueProvider(
            BindingSource bindingSource,
            IQueryCollection values,
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

    public class CaseQueryValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var valueProvider = new CaseQueryValueProvider(
                BindingSource.Query,
                context.ActionContext.HttpContext.Request.Query,
                System.Globalization.CultureInfo.CurrentCulture);

            context.ValueProviders.Add(valueProvider);

            return Task.CompletedTask;
        }
    }
}
