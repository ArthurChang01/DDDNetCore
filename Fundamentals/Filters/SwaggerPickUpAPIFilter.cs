using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Fundamentals.Filters
{
    public class SwaggerPickUpAPIFilter : IDocumentFilter
    {
        private readonly string _boundedContext;

        public SwaggerPickUpAPIFilter(string boundedContext)
        {
            this._boundedContext = boundedContext;
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths.Where(path => !path.Key.Contains(this._boundedContext)).ToList().ForEach(o =>
            {
                swaggerDoc.Paths.Remove(o.Key);
            });
        }
    }
}