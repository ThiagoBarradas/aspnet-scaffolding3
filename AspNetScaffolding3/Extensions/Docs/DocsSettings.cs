using System.Collections.Generic;

namespace AspNetScaffolding.Extensions.Docs
{
    public class DocsSettings
    {
        public bool Enabled { get; set; }

        public string Title { get; set; }

        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public string PathToReadme { get; set; }

        public string PathPrefix { get; set; }

        public string Version { get; set; }

        public string RedocUrl { get; set; }

        public string SwaggerJsonUrl { get; set; }

        public string SwaggerJsonTemplateUrl { get; set; }

        public IEnumerable<string> GetDocsFinalRoutes()
        {
            return new List<string>
            {
                this.SwaggerJsonUrl,
                this.RedocUrl
            };
        }
    }
}
