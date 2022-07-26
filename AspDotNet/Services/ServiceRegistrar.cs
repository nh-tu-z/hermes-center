using Microsoft.AspNetCore.Mvc;
using HermesCenter.Logger;
using HermesCenter.BackgroundServices;

namespace HermesCenter.Services
{
	public static class ServiceRegistrar
	{
		public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
		{
			return services;
		}

		public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
		{
			Logger.Environment logEnv = env.IsDevelopment()
				? Logger.Environment.Development
				: Logger.Environment.Production;
			return services.AddSingleton<ILogManager>(_ => new LogManager(logEnv));
		}

		public static IServiceCollection AddApiVersioningService(this IServiceCollection services) =>
			services.AddApiVersioning(config =>
			{
				config.DefaultApiVersion = new ApiVersion(1, 0);
				config.AssumeDefaultVersionWhenUnspecified = true;
				config.ReportApiVersions = true;
			});

		public static IServiceCollection AddServiceConfigurations(this IServiceCollection services, IConfiguration configuration) =>
			services; /* use .Configure<T>(config.GetSection(nameof(T) */

		public static IServiceCollection AddBackgroundServices(this IServiceCollection services) =>
			services
				.AddHostedService<WebSocketService>()
				.AddHostedService<QueueManagerService>();
	}
}

