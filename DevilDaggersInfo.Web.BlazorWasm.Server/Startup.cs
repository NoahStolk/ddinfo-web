using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;
using DevilDaggersInfo.Web.BlazorWasm.Server.Middleware;
using DevilDaggersInfo.Web.BlazorWasm.Server.NSwag;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using NJsonSchema;
using System.Globalization;

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
			options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), MySqlServerVersion.LatestSupportedServerVersion, providerOptions => providerOptions.EnableRetryOnFailure(5)));

		services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddControllersWithViews();

		services.AddRazorPages();

		services.AddTransient<ModArchiveAccessor>();
		services.AddTransient<ModArchiveProcessor>();
		services.AddTransient<ModScreenshotProcessor>();

		services.AddScoped<IUserService, UserService>();

		services.AddSingleton<BackgroundServiceMonitor>();
		services.AddSingleton<ResponseTimeMonitor>();

		services.AddSingleton<AuditLogger>();
		services.AddSingleton<IFileSystemService, FileSystemService>();

		services.AddSingleton<LeaderboardHistoryCache>();
		services.AddSingleton<LeaderboardStatisticsCache>();
		services.AddSingleton<ModArchiveCache>();
		services.AddSingleton<SpawnsetSummaryCache>();
		services.AddSingleton<SpawnsetHashCache>();

		services.AddHostedService<DiscordLogFlushBackgroundService>();

		services.AddDataProtection()
			.PersistKeysToFileSystem(new DirectoryInfo("keys"));

#if V5_RELEASE
		if (!WebHostEnvironment.IsDevelopment())
		{
			// TODO: ResponseTimeLoggerBackgroundService
			services.AddHostedService<BackgroundServiceLoggerBackgroundService>();
			services.AddHostedService<CacheLoggerBackgroundService>();
			services.AddHostedService<DatabaseLoggerBackgroundService>();
			services.AddHostedService<FileSystemLoggerBackgroundService>();
			services.AddHostedService<LeaderboardHistoryBackgroundService>();
		}
#endif

		// Use a transient for ToolHelper so we can update the Changelogs.json file without having to re-instantiate this.
		services.AddTransient<IToolHelper, ToolHelper>();

		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(jwtBearerOptions =>
			{
				jwtBearerOptions.RequireHttpsMetadata = true;
				jwtBearerOptions.SaveToken = true;
				jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtKey"])),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero,
				};
			});

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

			// V3
			.AddRedirect("Home/Index$", "/")
			.AddRedirect("Home$", "/")
			.AddRedirect("Home/Leaderboard$", "leaderboard")
			.AddRedirect("Home/Spawnsets$", "custom/spawnsets")
			.AddRedirect("Home/Donations$", "donations")

			// V3 wiki
			.AddRedirect("Home/Hands$", "wiki/upgrades")
			.AddRedirect("Home/Enemies$", "wiki/enemies")
			.AddRedirect("Home/Daggers$", "wiki/daggers")
			.AddRedirect("Home/Spawns$", "wiki/spawns")
			.AddRedirect("Daggers$", "wiki/daggers")
			.AddRedirect("Enemies$", "wiki/enemies")
			.AddRedirect("Spawns$", "wiki/spawns")
			.AddRedirect("Upgrades$", "wiki/upgrades")

			// V4 outdated
			.AddRedirect("Wiki/SpawnsetGuide$", "guides/survival-editor")
			.AddRedirect("Wiki/AssetGuide$", "guides/asset-editor")
			.AddRedirect("Leaderboard/UserSettings", "leaderboard/player-settings")

			// V4
			.AddRedirect("Leaderboard/PlayerSettings", "leaderboard/player-settings")
			.AddRedirect("Leaderboard/WorldRecordProgression", "leaderboard/world-record-progression")
			.AddRedirect("CustomLeaderboards", "custom/leaderboards")
			.AddRedirect("Mods", "custom/mods")
			.AddRedirect("Spawnsets", "custom/spawnsets")
			.AddRedirect("Tools/DevilDaggersAssetEditor", "tools/asset-editor")
			.AddRedirect("Tools/DevilDaggersCustomLeaderboards", "tools/custom-leaderboards")
			.AddRedirect("Tools/DevilDaggersSurvivalEditor", "tools/survival-editor")
			.AddRedirect("Wiki/Guides/SurvivalEditor$", "guides/survival-editor")
			.AddRedirect("Wiki/Guides/AssetEditor$", "guides/asset-editor")

			.Add(new RewriteRules());

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
		leaderboardStatisticsCache.Initiate();

		// Initiate dynamic caches.

		// SpawnsetHashCache does not need to be initiated as it is fast enough.
		// SpawnsetSummaryCache does not need to be initiated as it is fast enough.

		// TODO: LeaderboardHistoryCache might need to be initiated as the initial world record progression load is a little slow.

		/* The ModArchiveCache is initially very slow because it requires unzipping huge mod archive zip files.
		 * The idea to fix this; when adding data (based on a mod archive) to the ConcurrentBag, write this data to a JSON file as well, so it is not lost when the site shuts down.
		 * The cache then needs to be initiated here, by reading all the JSON files and populating the ConcurrentBag on start up. Effectively this is caching the cache.*/
		ModArchiveCache modArchiveCache = serviceProvider.GetRequiredService<ModArchiveCache>();
		modArchiveCache.LoadEntireFileCache();

		// Create roles if they don't exist.
		ApplicationDbContext dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
		bool anyChanges = false;
		foreach (string roleName in Roles.All)
		{
			if (!dbContext.Roles.Any(r => r.Name == roleName))
			{
				dbContext.Roles.Add(new RoleEntity { Name = roleName });
				anyChanges = true;
			}
		}

		if (anyChanges)
			dbContext.SaveChanges();
	}
}
