using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspNetScaffolding.Extensions.RequestKey
{
    public static class MvcBuilderExtension
    {
        public static void RegisterAssembliesForControllers(this IMvcBuilder mvc, IEnumerable<Assembly> assemblies)
        {
            if (assemblies?.Any() == true)
            {
                assemblies.ToList().ForEach(assembly => mvc.AddApplicationPart(assembly));
            }
        }

        public static void RegisterAssembliesForFluentValidation(this IMvcBuilder mvc, IEnumerable<Assembly> assemblies)
        {
            mvc.AddFluentValidation(fluent =>
            {
                fluent.RegisterValidatorsFromAssemblyContaining<Startup>();

                if (assemblies?.Any() == true)
                {
                    fluent.RegisterValidatorsFromAssemblies(assemblies);
                }
            });
        }
    }
}
