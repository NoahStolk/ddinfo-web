﻿using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.HostedServices;
using DevilDaggersWebsite.Middleware;
using DevilDaggersWebsite.Singletons;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;

namespace DevilDaggersWebsite.BlazorWasm.Server
{
	public class Startup
	{
		private const string _defaultCorsPolicy = nameof(_defaultCorsPolicy);

		public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
		{
			Configuration = configuration;
			WebHostEnvironment = webHostEnvironment;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment WebHostEnvironment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), MySqlServerVersion.LatestSupportedServerVersion, providerOptions => providerOptions.EnableRetryOnFailure(1)));

			services.AddDatabaseDeveloperPageExceptionFilter();

			services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddIdentityServer()
				.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

			services.AddAuthentication()
				.AddIdentityServerJwt();

			services.AddControllersWithViews();
			services.AddRazorPages();

			services.AddSingleton<BackgroundServiceMonitor>();
			services.AddSingleton<ResponseTimeMonitor>();

			services.AddSingleton<DiscordLogger>();
			services.AddSingleton<AuditLogger>();
			services.AddSingleton<LeaderboardHistoryCache>();
			services.AddSingleton<LeaderboardStatisticsCache>();
			services.AddSingleton<ModArchiveCache>();
			services.AddSingleton<SpawnsetDataCache>();
			services.AddSingleton<SpawnsetHashCache>();

			if (!WebHostEnvironment.IsDevelopment())
			{
				services.AddHostedService<BackgroundServiceLoggerBackgroundService>();
				services.AddHostedService<CacheLoggerBackgroundService>();
				services.AddHostedService<DatabaseLoggerBackgroundService>();
				services.AddHostedService<FileSystemLoggerBackgroundService>();
				services.AddHostedService<LeaderboardHistoryBackgroundService>();
				//services.AddHostedService<ResponseTimeLoggerBackgroundService>();
			}

			services.AddTransient<WorldRecordsHelper>();
			services.AddTransient<ModHelper>();
			services.AddTransient<SpawnsetHelper>();
			services.AddTransient<IToolHelper, ToolHelper>(); // TODO: Singleton?
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

			app.UseMiddleware<ResponseTimeMiddleware>();

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
				.AddRedirect("^Spawns$", "Wiki/Spawns")
				.AddRedirect("^Home/Spawns$", "Wiki/Spawns")
				.AddRedirect("^Wiki/SpawnsetGuide$", "Wiki/Guides/SurvivalEditor")
				.AddRedirect("^Wiki/AssetGuide$", "Wiki/Guides/AssetEditor")
				.AddRedirect("^Leaderboard/UserSettings", "Leaderboard/PlayerSettings");
			app.UseRewriter(options);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseCors(_defaultCorsPolicy);

			app.UseIdentityServer();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}
