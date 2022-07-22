using System.Diagnostics;
using System.Text.Json;

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
            services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddRouting(options => options.LowercaseUrls = true);

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
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
    }
}
