using Microsoft.AspNetCore.Mvc;
using HermesCenter.AssetDiscovery;
using HermesCenter.BackgroundServices;
using HermesCenter.Common.Configuration;
using HermesCenter.Interfaces;
using HermesCenter.Logger;

namespace HermesCenter.Services
{
	public static class ServiceRegistrar
	{
		public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddSingleton<IMLConnector, AzureMLConnector>()
				.AddSingleton<IAssetDiscoveryEngine, AssetDiscoveryEngine>()
				.AddScoped<IIntegrationService, IntegrationService>()
				.AddScoped<IRedisQueueService, RedisQueueService>();
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
			services
				.Configure<QueueConfig>(configuration.GetSection(nameof(QueueConfig)));

		public static IServiceCollection AddBackgroundServices(this IServiceCollection services) =>
			services
				.AddHostedService<WebSocketService>()
				.AddHostedService<QueueManagerService>()
				.AddHostedService<SyncAssetService>();
	}
}

