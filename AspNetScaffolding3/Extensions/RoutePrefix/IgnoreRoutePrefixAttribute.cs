using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace AspNetScaffolding.Extensions.RoutePrefix
{
    public class IgnoreRoutePrefixAttribute : Attribute, IActionFilter, IFilterMetadata 
    {
        public bool AllowMultiple => false;

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
