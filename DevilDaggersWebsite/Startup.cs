using CoreBase;
using DevilDaggersWebsite.Code.Bot;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Tasks;
using DevilDaggersWebsite.Code.Tasks.Scheduling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace DevilDaggersWebsite
{
	public class Startup : StartupAbstract
	{
		public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment env)
			: base(configuration, loggerFactory, env)
		{
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			ConfigureDbServices<ApplicationDbContext>(services);

			AddCommonCoreBaseServices(services);

			// TODO: Add all tasks using reflection?
			services.AddSingleton<IScheduledTask, CreateLeaderboardHistoryFileTask>();
			//services.AddSingleton<IScheduledTask, RetrieveEntireLeaderboardTask>();

			services.AddScheduler((sender, args) =>
			{
				Console.Write(args.Exception.Message);
				args.SetObserved();
			});

			services.AddHostedService<BotService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			CultureInfo cultureInfo = new CultureInfo("en-US");
			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

			// Do not change order of redirects.
			RewriteOptions options = new RewriteOptions()
				.AddRedirect("^Home/Index$", "Index")
				.AddRedirect("^Home/Leaderboard$", "Leaderboard")
				.AddRedirect("^Home/Spawnset/(.*)$", "Spawnset?spawnset=$1")
				.AddRedirect("^Home/Spawnsets$", "Spawnsets")
				.AddRedirect("^Home/SpawnsetsInfo$", "Spawnsets")
				.AddRedirect("^Home/Spawnset$", "Spawnset")
				.AddRedirect("^Home/Hands$", "Wiki/Upgrades")
				.AddRedirect("^Home/Enemies$", "Wiki/Enemies")
				.AddRedirect("^Home/Daggers$", "Wiki/Daggers")
				.AddRedirect("^Home/Donations$", "Donations")
				.AddRedirect("^Home$", "Index")
				.AddRedirect("^Upgrades$", "Wiki/Upgrades")
				.AddRedirect("^Enemies$", "Wiki/Enemies")
				.AddRedirect("^Daggers$", "Wiki/Daggers")
				.AddRedirect("^GetSpawnsets$", "Api/GetSpawnsets")
				.AddRedirect("^GetToolVersions$", "Api/GetToolVersions")
				.AddRedirect("^LeaderboardJson$", "Api/LeaderboardJson")
				.AddRedirect("^Spawns$", "Wiki/Spawns")
				.AddRedirect("^Home/Spawns$", "Wiki/Spawns")
				.AddRedirect("^DownloadSpawnset", "Api/DownloadSpawnset")
				.AddRedirect("^DownloadSpawnset?file=(.*)", "Api/DownloadSpawnset?file=$1");

			app.UseRewriter(options);

			ConfigureErrorPageAndSSL(app, env);

			app.UseStaticFiles();

			app.UseMvc();

			app.UseStatusCodePagesWithRedirects("/Error/{0}");
		}
	}
}