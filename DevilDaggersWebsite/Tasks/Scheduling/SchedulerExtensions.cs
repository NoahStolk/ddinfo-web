using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Core.Tasks.Scheduling
{
	public static class SchedulerExtensions
	{
		public static IServiceCollection AddScheduler(this IServiceCollection services)
		{
			return services.AddSingleton<IHostedService, SchedulerHostedService>();
		}

		public static IServiceCollection AddScheduler(this IServiceCollection services, EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler)
		{
			return services.AddSingleton<IHostedService, SchedulerHostedService>(serviceProvider =>
			{
				SchedulerHostedService instance = new SchedulerHostedService(serviceProvider.GetServices<IScheduledTask>());
				instance.UnobservedTaskException += unobservedTaskExceptionHandler;
				return instance;
			});
		}
	}
}