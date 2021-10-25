using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Logging.Discord;

public static class DiscordLoggerExtensions
{
	public static ILoggingBuilder AddDiscordLogger(this ILoggingBuilder builder, Action<DiscordLoggerConfiguration> configure)
	{
		builder.AddDiscordLogger();
		builder.Services.Configure(configure);

		return builder;
	}

	public static ILoggingBuilder AddDiscordLogger(this ILoggingBuilder builder)
	{
		builder.AddConfiguration();

		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DiscordLoggerProvider>());

		LoggerProviderOptions.RegisterProviderOptions<DiscordLoggerConfiguration, DiscordLoggerProvider>(builder.Services);

		return builder;
	}
}
