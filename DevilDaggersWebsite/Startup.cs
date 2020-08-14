// #define TEST_EXCEPTION_HANDLER
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Tasks;
using DevilDaggersWebsite.Code.Tasks.Scheduling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace DevilDaggersWebsite
{
	public class Startup
	{
		private const string defaultCorsPolicy = nameof(defaultCorsPolicy);

		private const string adminTestPolicy = nameof(adminTestPolicy);
		private const string adminTestRole = nameof(adminTestRole);

		private const string assetModsPolicy = nameof(assetModsPolicy);
		private const string assetModsRole = nameof(assetModsRole);

		private const string customLeaderboardsPolicy = nameof(customLeaderboardsPolicy);
		private const string customLeaderboardsRole = nameof(customLeaderboardsRole);

		private const string donationsPolicy = nameof(donationsPolicy);
		private const string donationsRole = nameof(donationsRole);

		private const string playersPolicy = nameof(playersPolicy);
		private const string playersRole = nameof(playersRole);

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(defaultCorsPolicy, builder => { builder.AllowAnyOrigin(); });
			});

			services.AddMvc();

			services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

			services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

			// TODO: Add all tasks using reflection?
			// services.AddSingleton<IScheduledTask, RetrieveEntireLeaderboardTask>();
			services.AddSingleton<IScheduledTask, CreateLeaderboardHistoryFileTask>();

			services.AddScoped<IUrlHelper>(factory => new UrlHelper(factory.GetService<IActionContextAccessor>().ActionContext));

			services.AddScheduler((sender, args) =>
			{
				Console.Write(args.Exception?.Message);
				args.SetObserved();
			});

			services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.Converters.Add(new StringEnumConverter());
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(adminTestPolicy, policy => policy.RequireRole(adminTestRole));
				options.AddPolicy(assetModsPolicy, policy => policy.RequireRole(assetModsRole));
				options.AddPolicy(customLeaderboardsPolicy, policy => policy.RequireRole(customLeaderboardsRole));
				options.AddPolicy(donationsPolicy, policy => policy.RequireRole(donationsRole));
				options.AddPolicy(playersPolicy, policy => policy.RequireRole(playersRole));
			});

			services.AddRazorPages().AddRazorPagesOptions(options =>
			{
				options.Conventions.AuthorizeFolder("/Admin/AdminTests", adminTestPolicy);
				options.Conventions.AuthorizeFolder("/Admin/AssetMods", assetModsPolicy);
				options.Conventions.AuthorizeFolder("/Admin/CustomEntries", customLeaderboardsPolicy); // Maybe only allow admin here.
				options.Conventions.AuthorizeFolder("/Admin/CustomLeaderboardCategories", customLeaderboardsPolicy); // Maybe only allow admin here.
				options.Conventions.AuthorizeFolder("/Admin/CustomLeaderboards", customLeaderboardsPolicy);
				options.Conventions.AuthorizeFolder("/Admin/Donations", donationsPolicy);
				options.Conventions.AuthorizeFolder("/Admin/Players", playersPolicy);
				options.Conventions.AuthorizeFolder("/Admin/Titles", playersPolicy);
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

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
				.AddRedirect("^Wiki/SpawnsetGuide$", "Wiki/Guides/SurvivalEditor")
				.AddRedirect("^Wiki/AssetGuide$", "Wiki/Guides/AssetEditor")
				.AddRedirect("^DownloadSpawnset", "Api/DownloadSpawnset")
				.AddRedirect("^DownloadSpawnset?file=(.*)", "Api/DownloadSpawnset?file=$1");
			app.UseRewriter(options);

			if (env.IsDevelopment())
			{
#if TEST_EXCEPTION_HANDLER
				app.UseExceptionHandler("/Error");
#else
				app.UseDeveloperExceptionPage();
#endif
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseStatusCodePagesWithReExecute("/Error/{0}");
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseCors(defaultCorsPolicy);

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
			});
		}
	}
}