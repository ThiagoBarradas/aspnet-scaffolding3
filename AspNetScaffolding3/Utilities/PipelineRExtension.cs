using AspNetScaffolding;
using Microsoft.AspNetCore.Mvc;
using PackUtils;
using PipelineR;
using System.Linq;
using WebApi.Models.Response;

namespace AspNetScaffolding3.Utilities
{
    public static class PipelineRExtension
    {
        public static IActionResult AsActionResult(this RequestHandlerResult requestHandlerResult)
        {
            var responseObject = requestHandlerResult.Result();

            if (requestHandlerResult.Errors != null)
            {
                var errorsResponse = new ErrorsResponse();

                foreach (var error in requestHandlerResult.Errors)
                {
                    string property = null;

                    if (error.Property != null)
                    {
                        var parts = error.Property.Split(".").ToList();
                        
                        if (parts.Count > 1)
                        {
                            parts.RemoveAt(0);
                        }

                        var finalParts = parts.Select(i => i.ToCase(Api.ApiSettings.JsonSerializerString));
                        property = string.Join(".", finalParts);
                    }

                    errorsResponse.AddError(error.Message, property);
                }

                responseObject = errorsResponse;
            }

            return new ObjectResult(responseObject)
            {
                StatusCode = requestHandlerResult.StatusCode
            };
        }

    }
}
