using HellEngine.Core.Services;
using HellEngine.Core.Services.Scripting;
using HellEngine.Utils.Configuration.ServiceRegistrator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HellGame.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var configPathBuilderFactory =
                HellEngine.Core.AssemblyEntryPoint.ConfigPathBuilderFactoryMethod();
            services.AddOptions<HelloWorlderOptions>()
                .Bind(Configuration.GetSection(
                    configPathBuilderFactory().Add(HelloWorlderOptions.Path).Build()));
            services.AddOptions<ScriptHostOptions>()
                .Bind(Configuration.GetSection(
                    configPathBuilderFactory().Add(ScriptHostOptions.Path).Build()));

            var appServicesRegistrator = new ApplicationServicesRegistrator();
            appServicesRegistrator.RegisterApplicationServices(
                services,
                typeof(HellEngine.Core.AssemblyEntryPoint).Assembly);

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            HellEngine.Core.AssemblyEntryPoint.InitSdk(app.ApplicationServices);
        }
    }
}
