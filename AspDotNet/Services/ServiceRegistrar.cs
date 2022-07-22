using System;

namespace HermesCenter.Services
{
	public static class ServiceRegistrar
	{
		public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
		{
			return services;
		}

		public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment Env)
		{

			return services;
		}
	}
}

