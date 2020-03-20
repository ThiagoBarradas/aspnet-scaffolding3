using AspNetScaffolding.Extensions.Cors;
using AspNetScaffolding.Extensions.GracefullShutdown;
using AspNetScaffolding.Extensions.JsonSerializer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PackUtils;
using System;
using System.IO;
using System.Linq;
using WebApi.Models.Exceptions;
using WebApi.Models.Response;

namespace AspNetScaffolding.Controllers
{
    [EnableCors(CorsServiceExtension.CorsName)]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
            this.ValidationShutdown();
        }

        protected IActionResult CreateJsonResponse(ApiResponse response)
        {
            IActionResult result;

            if (response.Content != null)
            {
                result = new JsonResult(response.Content)
                {
                    StatusCode = (int)response.StatusCode
                };
            }
            else
            {
                result = new StatusCodeResult((int)response.StatusCode);
                Response.ContentType = "application/json";
            }

            if (response.Headers != null)
            {
                foreach (var header in response.Headers)
                {
                    Response.Headers[header.Key] = header.Value;
                }
            }

            return result;
        }

        protected void ValidateSignatureFromHeaderWithContent(string secretKey, string headerName)
        {
            var result = false;
            var signature = Request.Headers[headerName].FirstOrDefault() ?? string.Empty;

            if (Request.Body.CanRead &&
                Request.Body.CanSeek)
            {
                try
                {
                    Request.EnableBuffering();
                }
                catch (Exception) { }

                MemoryStream stream = new MemoryStream();
                Request.Body.Seek(0, SeekOrigin.Begin);
                Request.Body.CopyTo(stream);
                Request.Body.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(stream))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    var content = reader.ReadToEnd();
                    result = SignatureUtility.ValidateSignature(signature, secretKey, content);
                }
            }

            if (result == false)
            {
                throw new UnauthorizedException();
            }
        }

        protected void Validate<TRequest>(TRequest request) where TRequest : class, new()
        {
            if (ModelState.IsValid == false)
            {
                ErrorsResponse errors = CastModelValidationResultToErrorsResponse(ModelState);
                throw new BadRequestException(errors);
            }
        }

        private ErrorsResponse CastModelValidationResultToErrorsResponse(ModelStateDictionary modelState)
        {
            ErrorsResponse errorsResponse = new ErrorsResponse();

            foreach (var errorPerProperty in modelState)
            {
                foreach (var errorDetail in errorPerProperty.Value.Errors)
                {
                    var propertyName = string.Join(".",
                        errorPerProperty.Key.Split(".")
                        .Select(r => r.GetValueConsideringCurrentCase()));

                    errorsResponse.AddError(errorDetail.ErrorMessage, propertyName);
                }
            }

            return errorsResponse;
        }
    }
}
