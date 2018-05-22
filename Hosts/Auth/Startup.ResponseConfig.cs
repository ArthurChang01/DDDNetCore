using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;

namespace Auth
{
    public partial class Startup
    {
        public void ConfigureResponse(IServiceCollection services)
        {
            services.AddResponseCompression(opt => opt.Providers.Add<GzipCompressionProvider>());
            services.Configure<GzipCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest);
        }
    }
}