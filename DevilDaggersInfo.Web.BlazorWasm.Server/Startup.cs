using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;
using DevilDaggersInfo.Web.BlazorWasm.Server.Middleware;
using DevilDaggersInfo.Web.BlazorWasm.Server.NSwag;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Rewrite;
using NJsonSchema;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace DevilDaggersInfo.Web.BlazorWasm.Server;

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
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), MySqlServerVersion.LatestSupportedServerVersion, providerOptions => providerOptions.EnableRetryOnFailure(1)));

		services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();

		services.AddIdentityServer()
			.AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
			{
				options.IdentityResources["openid"].UserClaims.Add("role");
				options.ApiResources.Single().UserClaims.Add("role");
			});

		JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

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
		services.AddSingleton<SpawnsetSummaryCache>();
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

		// Use a transient for ToolHelper so we can update the Tools.json file without having to re-instantiate this.
		services.AddTransient<IToolHelper, ToolHelper>();
		services.AddTransient<IFileSystemService, FileSystemService>();

		services.AddSwaggerDocument(config =>
		{
			config.PostProcess = document =>
			{
				document.Info.Title = "DevilDaggers.Info API";
				document.Info.Contact = new()
				{
					Name = "Noah Stolk",
					Url = "//noahstolk.com/",
				};
			};
			config.DocumentProcessors.Add(new PublicApiDocumentProcessor());
			config.SchemaType = SchemaType.OpenApi3;
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
			app.UseDeveloperExceptionPage();
			app.UseMigrationsEndPoint();
			app.UseWebAssemblyDebugging();
		}
		else
		{
			app.UseExceptionHandler("/Error");
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

		app.UseOpenApi();
		app.UseSwaggerUi3();

		// Initiate static caches.
		LeaderboardStatisticsCache leaderboardStatisticsCache = serviceProvider.GetRequiredService<LeaderboardStatisticsCache>();
		Task task = leaderboardStatisticsCache.Initiate();
		task.Wait();

		// Initiate dynamic caches.

		// SpawnsetHashCache does not need to be initiated as it is fast enough.
		// SpawnsetSummaryCache does not need to be initiated as it is fast enough.

		// TODO: LeaderboardHistoryCache might need to be initiated as the initial world record progression load is a little slow.

		/* The ModArchiveCache is initially very slow because it requires unzipping huge mod archive zip files.
		 * The idea to fix this; when adding data (based on a mod archive) to the ConcurrentBag, write this data to a JSON file as well, so it is not lost when the site shuts down.
		 * The cache then needs to be initiated here, by reading all the JSON files and populating the ConcurrentBag on start up. Effectively this is caching the cache.*/
		//ModArchiveCache modArchiveCache = serviceProvider.GetRequiredService<ModArchiveCache>();
		//modArchiveCache.LoadEntireFileCache();

		CreateRoles(serviceProvider).Wait();
	}

	private static async Task CreateRoles(IServiceProvider serviceProvider)
	{
		RoleManager<IdentityRole>? roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
		foreach (string? roleName in Roles.All)
		{
			bool roleExist = await roleManager.RoleExistsAsync(roleName);
			if (!roleExist)
				await roleManager.CreateAsync(new IdentityRole(roleName));
		}
	}
}
