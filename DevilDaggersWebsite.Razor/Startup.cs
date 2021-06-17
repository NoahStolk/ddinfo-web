// #define TEST_EXCEPTION_HANDLER
using DevilDaggersWebsite.Authorization;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Middleware;
using DevilDaggersWebsite.Singletons;
using DevilDaggersWebsite.Tasks;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor
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

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options => options.AddPolicy(_defaultCorsPolicy, builder => builder.AllowAnyOrigin()));

			services.AddMvc();

			services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), MySqlServerVersion.LatestSupportedServerVersion, providerOptions => providerOptions.EnableRetryOnFailure(1)));
			services.AddDefaultIdentity<IdentityUser>(options =>
				{
					options.SignIn.RequireConfirmedAccount = true;
					options.User.RequireUniqueEmail = true;
				})
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddSingleton<ResponseTimeLogger>();

			if (!WebHostEnvironment.IsDevelopment())
				services.AddHostedService<LeaderboardHistoryBackgroundService>();

			services.AddTransient<WorldRecordsHelper>();
			services.AddTransient<ModHelper>();
			services.AddTransient<SpawnsetHelper>();
			services.AddTransient<IToolHelper, ToolHelper>();

			services
				.AddControllers()
				.AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()))
				.AddRazorRuntimeCompilation();

			services.AddAuthorization(options =>
			{
				foreach (KeyValuePair<string, string> kvp in AuthorizationManager.PolicyToRoleMapper)
					options.AddPolicy(kvp.Key, policy => policy.RequireRole(kvp.Value));
			});

			services.AddRazorPages().AddRazorPagesOptions(options =>
			{
				foreach (KeyValuePair<string, string> kvp in AuthorizationManager.FolderToPolicyMapper)
					options.Conventions.AuthorizeFolder(kvp.Key, kvp.Value);
			});

			services.AddSwaggerDocument(config => config.PostProcess = document =>
			{
				document.Info.Title = "DevilDaggers.Info API";
				document.Info.Contact = new()
				{
					Name = "Noah Stolk",
					Url = "//noahstolk.com/",
				};
			});
		}

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

			app.UseCors(_defaultCorsPolicy);

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
			});

			app.UseOpenApi();
			app.UseSwaggerUi3();

			Task task = serviceProvider.CreateRolesAndAdminUser(Configuration.GetSection("AdminUser")["Email"]);
			task.Wait();

			// Initiate static caches.
			task = LeaderboardStatisticsCache.Instance.Initiate(env);
			task.Wait();

			// Initiate dynamic caches.

			// SpawnsetDataCache does not need to be initiated as it is fast enough.
			// SpawnsetHashCache does not need to be initiated as it is fast enough.

			// TODO: LeaderboardHistoryCache might need to be initiated as the initial world record progression load is a little slow.

			/* The ModArchiveCache is initially very slow because it requires unzipping huge mod archive zip files.
			 * The idea to fix this; when adding data (based on a mod archive) to the ConcurrentBag, write this data to a JSON file as well, so it is not lost when the site shuts down.
			 * The cache then needs to be initiated here, by reading all the JSON files and populating the ConcurrentBag on start up. Effectively this is caching the cache.*/
			ModArchiveCache.Instance.LoadEntireFileCache(env);
		}
	}
}
