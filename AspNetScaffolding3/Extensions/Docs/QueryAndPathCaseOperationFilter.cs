using AspNetScaffolding.Extensions.JsonSerializer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Text.RegularExpressions;

namespace AspNetScaffolding.Extensions.Docs
{
    public class QueryAndPathCaseOperationFilter : IOperationFilter
    {
        public QueryAndPathCaseOperationFilter()
        {
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                foreach (var param in operation.Parameters.Where(p => p.In == ParameterLocation.Query || p.In == ParameterLocation.Path))
                {
                    param.Name = param.Name.GetValueConsideringCurrentCase();
                }

                var grouped = operation.Parameters
                    .Where(p => p.In == ParameterLocation.Query || p.In == ParameterLocation.Path)
                    .GroupBy(r => r.Name);

                var queryAndPath = grouped.Select(r => r.OrderBy(p => p.In).First()).ToList();

                operation.Parameters.ToList()
                    .RemoveAll(p => p.In == ParameterLocation.Query || p.In == ParameterLocation.Path);

                operation.Parameters.ToList().AddRange(queryAndPath);
            }

            if (context.ApiDescription.ParameterDescriptions != null)
            {
                foreach (var param in context.ApiDescription.ParameterDescriptions)
                {
                    param.Name = param.Name.GetValueConsideringCurrentCase();
                }
            }

            var path = context.ApiDescription.RelativePath;
            var matches = Regex.Matches(path, @"[{]{1}[\w\\_]*[}]{1}");

            foreach (var match in matches)
            {
                var oldValue = match.ToString();
                var newValue = "{" + oldValue.TrimStart('{').TrimEnd('}').GetValueConsideringCurrentCase() + "}";
                path = path.Replace(oldValue, newValue);
            }

            context.ApiDescription.RelativePath = path;
        }
    }
}
