using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebAdvertisements.SearchApi.Extensions;
using WebAdvertisements.SearchApi.Services;

namespace WebAdvertisements.SearchApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddElasticSearch(Configuration);

            services.AddScoped<ISearchService, SearchService>();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllOrigin", policy => policy.WithOrigins("*").AllowAnyHeader());
            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseHealthChecks("/health");

            loggerFactory.AddAWSProvider(Configuration.GetAWSLoggingConfigSection(),
                formatter:
                (loglevel, message, exception) =>
                $"[{DateTime.Now} {loglevel} {message} {exception?.Message} {exception?.StackTrace}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
