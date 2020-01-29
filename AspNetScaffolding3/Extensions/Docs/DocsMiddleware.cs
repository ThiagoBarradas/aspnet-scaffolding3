using Microsoft.AspNetCore.Builder;

namespace AspNetScaffolding.Extensions.Docs
{
    public static class DocsMiddlewareExtension
    {
        public static void UseScaffoldingSwagger(this IApplicationBuilder app)
        {
            if (DocsServiceExtension.DocsSettings?.Enabled == true)
            {
                var title = DocsServiceExtension.DocsSettings?.Title ?? "API Reference";

                app.UseStaticFiles();

                app.UseSwagger(c =>
                {
                    c.RouteTemplate = DocsServiceExtension.DocsSettings.SwaggerJsonTemplateUrl.TrimStart('/');
                });

                app.UseReDoc(c =>
                {
                    c.RoutePrefix = DocsServiceExtension.DocsSettings.RedocUrl.TrimStart('/');
                    c.SpecUrl = DocsServiceExtension.DocsSettings.SwaggerJsonUrl;
                    c.DocumentTitle = title;
                });
            }
        }
    }
}
