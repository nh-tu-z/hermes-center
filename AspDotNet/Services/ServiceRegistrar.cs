using HermesCenter.Logger;

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
	}
}

