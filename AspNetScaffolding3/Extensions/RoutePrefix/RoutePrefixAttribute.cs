using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace AspNetScaffolding.Extensions.RoutePrefix
{
    public class RoutePrefixConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel CentralPrefix;

        public RoutePrefixConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            this.CentralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(CentralPrefix,
                            selectorModel.AttributeRouteModel);
                    }
                }

                var unmatchedSelectors = controller.Selectors.Where(x =>  x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = CentralPrefix;
                    }
                }
            }
        }
    }
}
