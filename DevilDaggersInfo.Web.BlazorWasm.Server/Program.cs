using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersInfo.Web.BlazorWasm.Server.Logging.Discord;

namespace DevilDaggersInfo.Web.BlazorWasm.Server;

public static class Program
{
	public static void Main(string[] args)
		=> CreateHostBuilder(args).Build().Run();

	public static IHostBuilder CreateHostBuilder(string[] args)
	{
		return Host.CreateDefaultBuilder(args)
			.ConfigureLogging(builder => builder.ClearProviders().AddDiscordLogger())
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStaticWebAssets();
				webBuilder.UseStartup<Startup>();
			})
			.ConfigureServices(services => services.AddHostedService<DdInfoDiscordBotService>());
	}
}
