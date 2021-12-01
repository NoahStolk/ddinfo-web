using DevilDaggersInfo.Web.BlazorWasm.Server;
using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersInfo.Web.BlazorWasm.Server.Logging.Discord;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
	.ConfigureLogging(builder => builder.ClearProviders().AddDiscordLogger())
	.ConfigureWebHostDefaults(webBuilder =>
	{
		webBuilder.UseStaticWebAssets();
		webBuilder.UseStartup<Startup>();
	})
	.ConfigureServices(services => services.AddHostedService<DdInfoDiscordBotService>());

IHost app = builder.Build();
app.Run();
