using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Linq;

namespace AspNetScaffolding.Extensions.CultureInfo
{
    public static class CultureInfoMiddlewareExtension
    {
        public static void UseScaffoldingRequestLocalization(this IApplicationBuilder app, string[] acceptedsLanguages)
        {
            if (acceptedsLanguages?.Any() == true)
            {
                app.UseRequestLocalization(options =>
                {
                    options.AddSupportedCultures(acceptedsLanguages);
                    options.AddSupportedUICultures(acceptedsLanguages);
                    options.SetDefaultCulture(acceptedsLanguages.First());
                    options.RequestCultureProviders = new List<IRequestCultureProvider>
                    {
                        new AcceptLanguageHeaderRequestCultureProvider { Options = options }
                    };
                });
            }
        }
    }
}
