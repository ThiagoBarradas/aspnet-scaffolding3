using AspNetScaffolding.Extensions.JsonSerializer;
using AspNetScaffolding.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using PackUtils;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AspNetScaffolding.Extensions.Docs
{
    public static class DocsServiceExtension
    {
        public static DocsSettings DocsSettings { get; set; }

        public static void SetupSwaggerDocs(
            this IServiceCollection services,
            DocsSettings docsSettings,
            ApiSettings apiSettings)
        {
            DocsSettings = docsSettings;

            if (DocsSettings?.Enabled == true)
            {
                DocsSettings.Version = apiSettings.Version;
                DocsSettings.PathPrefix = apiSettings.PathPrefix;
                GenerateSwaggerUrl();

                services.AddSwaggerGen(options =>
                {
                    string readme = null;
                    try
                    {
                        if (string.IsNullOrWhiteSpace(DocsSettings.PathToReadme) == false)
                        {
                            readme = File.ReadAllText(DocsSettings.PathToReadme);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"[ERROR] Swagger markdown ({DocsSettings.PathToReadme}) could not be loaded.");
                    }

                    switch (apiSettings.JsonSerializer)
                    {
                        case JsonSerializerEnum.Camelcase:
                            options.SchemaFilter<CamelEnumSchemaFilter>();
                            break;
                        case JsonSerializerEnum.Snakecase:
                            options.SchemaFilter<SnakeEnumSchemaFilter>();
                            break;
                        case JsonSerializerEnum.Lowercase:
                            options.SchemaFilter<LowerEnumSchemaFilter>();
                            break;
                    }

                    options.SchemaFilter<SnakeEnumSchemaFilter2>();

                    options.CustomSchemaIds(x => x.FullName);
                    options.CustomOperationIds(apiDesc =>
                    {
                        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) 
                            ? methodInfo.Name : null;
                    });

                    options.OperationFilter<QueryAndPathCaseOperationFilter>();
                    options.SwaggerDoc(apiSettings.Version, new OpenApiInfo
                    {
                        Title = DocsSettings.Title,
                        Version = apiSettings.Version,
                        Description = readme,
                        Contact = new OpenApiContact
                        {
                            Name = DocsSettings.AuthorName,
                            Email = DocsSettings.AuthorEmail
                        }
                    });
                });
                services.AddSwaggerGenNewtonsoftSupport();
            }
        }

        private static void GenerateSwaggerUrl()
        {
            string swaggerJsonPath = "/swagger/{documentName}/swagger.json";
            string finalPath = string.Format("/swagger/{0}/swagger.json", DocsServiceExtension.DocsSettings.Version);
            string docsPath = "/docs";

            if (DocsServiceExtension.DocsSettings.PathPrefix?.Contains("{version}", StringComparison.OrdinalIgnoreCase) == true)
            {
                swaggerJsonPath = DocsServiceExtension.DocsSettings.PathPrefix.Replace("{version}", "{documentName}", StringComparison.OrdinalIgnoreCase).Trim('/');
                swaggerJsonPath = string.Format("/{0}/swagger.json", swaggerJsonPath);
                finalPath = swaggerJsonPath.Replace("{documentName}", DocsServiceExtension.DocsSettings.Version);
                docsPath = finalPath.Replace("swagger.json", "docs");
            }
            else if (string.IsNullOrWhiteSpace(DocsServiceExtension.DocsSettings?.PathPrefix) == false)
            {
                var prefix = string.Format("/{0}/", DocsServiceExtension.DocsSettings.PathPrefix.Trim('/'));
                swaggerJsonPath = prefix + swaggerJsonPath.TrimStart('/');
                finalPath = prefix + finalPath.TrimStart('/');
                docsPath = prefix + docsPath.TrimStart('/');
            }

            DocsServiceExtension.DocsSettings.SwaggerJsonTemplateUrl = swaggerJsonPath;
            DocsServiceExtension.DocsSettings.SwaggerJsonUrl = finalPath;
            DocsServiceExtension.DocsSettings.RedocUrl = docsPath;
        }
    }

    public class SnakeEnumSchemaFilter2 : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Enum.Count > 0)
            {
                IList<IOpenApiAny> results = new List<IOpenApiAny>();
                var enumValues = Enum.GetValues(context.Type);
                foreach (var enumValue in enumValues)
                {
                    results.Add(new OpenApiString(enumValue.ToString().ToSnakeCase()));
                }

                schema.Enum = results;
                schema.Type = "string";
                schema.Format = null;
            }
        }
    }
}