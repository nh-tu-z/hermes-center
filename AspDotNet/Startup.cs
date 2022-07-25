using System.Diagnostics;
using System.Text.Json;
using HermesCenter.Services;

namespace MyFirstCoreApp
{
    public class Startup
    {
        #region App Config Variables

        protected const string _version = "v1";
        protected const string _appName = "Cranium Security Center Admin API";
        protected const string _allowAllCorsPolicyName = "AllowAll";

        #endregion App Config Variables

        public IWebHostEnvironment Env { get; }
        protected IConfiguration Configuration { get; set; }

        public Startup(IWebHostEnvironment env)
        {
            #region Debug
            // cofig log in output tab for tracing
            Debug.IndentLevel = 3;
            Debug.WriteLine("StartUp");
            #endregion

            Env = env; // Microsoft.AspNetCore.Hosting.HostingEnvironment
            Configuration = GetBuilder().Build(); // Microsoft.Extensions.Configuration.ConfigurationRoot
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddSwagger(services);

            services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddRouting(options => options.LowercaseUrls = true);

            services
                .AddLogger(Configuration, Env)
                .AddApiVersioningService()
                .AddServiceConfigurations(Configuration)
                .AddBackgroundServices();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Ref - https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.applicationbuilder?view=aspnetcore-6.0
        public void Configure(IApplicationBuilder app /* Microsoft.AspNetCore.Builder.ApplicationBuilder */, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("swagger/v1/swagger.json", $"{_appName} {_version}"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }

        public virtual IConfigurationBuilder GetBuilder()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Env.ContentRootPath)
                .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddJsonFile($"appsettings.{Env.EnvironmentName}.json",
                    optional: true)
                .AddEnvironmentVariables();

            return config;
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_version, new Microsoft.OpenApi.Models.OpenApiInfo() { Title = _appName, Version = _version });
            });
        }
    }
}
