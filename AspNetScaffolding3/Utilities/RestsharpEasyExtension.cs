using Microsoft.AspNetCore.Mvc;
using RestSharp.Easy.Models;
using WebApi.Models.Response;

namespace AspNetScaffolding.Utilities
{
    public static class RestsharpEasyExtension
    {
        public static IActionResult AsActionResult<TResponse>(this BaseResponse<TResponse, ErrorsResponse> response)
        {
            object responseObj = response.Data;

            if (responseObj == null || !response.Is2XX)
            {
                responseObj = response.Error;
            }

            return new ObjectResult(responseObj)
            {
                StatusCode = (int) response.StatusCode
            };
        }
    }
}
