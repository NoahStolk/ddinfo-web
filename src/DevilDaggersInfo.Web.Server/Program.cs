using DevilDaggersInfo.Web.Server.Clients.Clubber;
using DevilDaggersInfo.Web.Server.Clients.Leaderboard;
using DevilDaggersInfo.Web.Server.Configuration;
using DevilDaggersInfo.Web.Server.Domain.Configuration;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Extensions;
using DevilDaggersInfo.Web.Server.HostedServices;
using DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersInfo.Web.Server.Middleware;
using DevilDaggersInfo.Web.Server.RewriteRules;
using DevilDaggersInfo.Web.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;

const string defaultCorsPolicy = "_defaultCorsPolicy";

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(o =>
{
	o.TracesSampleRate = 0.03;
	o.MinimumEventLevel = LogLevel.Information;
});
builder.WebHost.UseStaticWebAssets();

builder.AddValidatedOptions<AuthenticationOptions>("Authentication");
builder.AddValidatedOptions<CustomLeaderboardsOptions>("CustomLeaderboards");
builder.AddValidatedOptions<DiscordOptions>("Discord");
builder.AddValidatedOptions<MySqlOptions>("MySql");

builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

builder.Services.AddCors(options => options.AddPolicy(defaultCorsPolicy, corsBuilder => corsBuilder.AllowAnyOrigin().AllowAnyMethod()));

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
	MySqlOptions mySqlOptions = sp.GetRequiredService<IOptions<MySqlOptions>>().Value;
	options.UseMySql(mySqlOptions.ConnectionString, MySqlServerVersion.LatestSupportedServerVersion, providerOptions => providerOptions.EnableRetryOnFailure(5));
	options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

// Domain services
builder.Services.AddScoped<CustomEntryProcessor>();
builder.Services.AddSingleton<IFileSystemService, FileSystemService>();
builder.Services.AddTransient<ModArchiveAccessor>();
builder.Services.AddTransient<ModArchiveProcessor>();
builder.Services.AddTransient<ModScreenshotProcessor>();
builder.Services.AddScoped<UserManager>();

// Repositories
builder.Services.AddTransient<CustomEntryRepository>();
builder.Services.AddTransient<CustomLeaderboardRepository>();
builder.Services.AddTransient<MarkerRepository>();
builder.Services.AddTransient<PlayerHistoryRepository>();
builder.Services.AddTransient<PlayerRepository>();

// Main domain services
builder.Services.AddScoped<DevilDaggersInfo.Web.Server.Domain.Main.Services.AuthenticationService>();
builder.Services.AddScoped<DevilDaggersInfo.Web.Server.Domain.Main.Services.PlayerProfileService>();

// Main repositories
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Main.Repositories.DonationRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Main.Repositories.LeaderboardHistoryStatisticsRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Main.Repositories.PlayerCustomLeaderboardStatisticsRepository>();
builder.Services.AddScoped<DevilDaggersInfo.Web.Server.Domain.Main.Repositories.PlayerProfileRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Main.Repositories.WorldRecordRepository>();

// Admin domain services
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Services.CustomEntryService>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Services.CustomLeaderboardService>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Services.DonationService>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Services.MarkerService>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Services.ModService>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Services.PlayerService>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Services.SpawnsetService>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Services.UserService>();

// Admin repositories
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Repositories.CustomEntryRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Repositories.CustomLeaderboardRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Repositories.DonationRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Repositories.MarkerRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Repositories.ModRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Repositories.PlayerRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Repositories.SpawnsetRepository>();
builder.Services.AddTransient<DevilDaggersInfo.Web.Server.Domain.Admin.Repositories.UserRepository>();

// Utilities
builder.Services.AddSingleton<LeaderboardResponseParser>();

// Monitoring
builder.Services.AddSingleton<BackgroundServiceMonitor>();
builder.Services.AddSingleton<ICustomLeaderboardHighscoreLogger, CustomLeaderboardHighscoreLogger>();
builder.Services.AddSingleton<ICustomLeaderboardSubmissionLogger, CustomLeaderboardSubmissionLogger>();
builder.Services.AddSingleton<ILogContainerService, LogContainerService>();

// Caching services
builder.Services.AddSingleton<ILeaderboardHistoryCache, LeaderboardHistoryCache>();
builder.Services.AddSingleton<LeaderboardStatisticsCache>();
builder.Services.AddSingleton<ModArchiveCache>();

// HTTP services
builder.Services.AddHttpClient<ClubberClient>();
builder.Services.AddHttpClient<IDdLeaderboardService, DdLeaderboardService>();

// Register this background service first, so it exists last. We want to log when the application exits.
builder.Services.AddHostedService<DiscordBotService>();
builder.Services.AddHostedService<DiscordLogFlushBackgroundService>();

if (!builder.Environment.IsDevelopment())
{
	builder.Services.AddHostedService<DiscordUserIdFetchBackgroundService>();
	builder.Services.AddHostedService<LeaderboardHistoryBackgroundService>();
	builder.Services.AddHostedService<PlayerNameFetchBackgroundService>();
}

// Hosted service that runs once after startup.
builder.Services.AddHostedService<StartupCacheHostedService>();

builder.Services.AddDataProtection()
	.PersistKeysToFileSystem(new DirectoryInfo("keys"));

builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { });

builder.AddSwaggerDocument("Main", "This is the main API for DevilDaggers.info. **WARNING:** It is not recommended to use these endpoints as they may change at any time based on the website client requirements. Use at your own risk.");
builder.AddSwaggerDocument("Admin", "This is the admin API for DevilDaggers.info. Requires an authenticated and authorized user.");

builder.AddSwaggerDocument("Dd", "**WARNING:** This API is intended to be used by Devil Daggers only.");

builder.AddSwaggerDocument("Tools", "**WARNING:** This API is intended to be used by ddinfo tools only.");
builder.AddSwaggerDocument("Ddae", "**WARNING:** This API is intended to be used by Devil Daggers Asset Editor only.");
builder.AddSwaggerDocument("Ddse", "**WARNING:** This API is intended to be used by Devil Daggers Survival Editor only.");

builder.AddSwaggerDocument("DdLive", "**WARNING:** This API is intended to be used by DDLIVE only.");
builder.AddSwaggerDocument("DdstatsRust", "**WARNING:** This API is intended to be used by ddstats-rust only.");
builder.AddSwaggerDocument("Clubber", "**WARNING:** This API is intended to be used by Clubber only.");

WebApplication app = builder.Build();

// Do not change order of redirects.
RewriteOptions options = new RewriteOptions()

	// V3
	.AddRedirect("^Home/Index$", "/")
	.AddRedirect("^Home$", "/")
	.AddRedirect("^Home/Leaderboard$", "leaderboard")
	.AddRedirect("^Home/Spawnsets$", "custom/spawnsets")
	.AddRedirect("^Home/Donations$", "donations")

	// V3 wiki
	.AddRedirect("^Home/Hands$", "wiki/upgrades")
	.AddRedirect("^Home/Enemies$", "wiki/enemies")
	.AddRedirect("^Home/Daggers$", "wiki/daggers")
	.AddRedirect("^Home/Spawns$", "wiki/spawns")
	.AddRedirect("^Daggers$", "wiki/daggers")
	.AddRedirect("^Enemies$", "wiki/enemies")
	.AddRedirect("^Spawns$", "wiki/spawns")
	.AddRedirect("^Upgrades$", "wiki/upgrades")

	// V4 outdated
	.AddRedirect("^Wiki/SpawnsetGuide$", "guides/creating-spawnsets")
	.AddRedirect("^Wiki/AssetGuide$", "guides/creating-mods")
	.AddRedirect("^Leaderboard/UserSettings", "leaderboard/player-settings")

	// V4
	.AddRedirect("^Leaderboard/PlayerSettings", "leaderboard/player-settings")
	.AddRedirect("^Leaderboard/WorldRecordProgression", "leaderboard/world-record-progression")
	.AddRedirect("^CustomLeaderboards$", "custom/leaderboards")
	.AddRedirect("^Mods", "custom/mods")
	.AddRedirect("^Spawnsets", "custom/spawnsets")
	.AddRedirect("^Tools/DevilDaggersAssetEditor", "tools/asset-editor")
	.AddRedirect("^Tools/DevilDaggersCustomLeaderboards", "tools/custom-leaderboards")
	.AddRedirect("^Tools/DevilDaggersSurvivalEditor", "tools/survival-editor")
	.AddRedirect("^Wiki/Guides/SurvivalEditor$", "guides/creating-spawnsets")
	.AddRedirect("^Wiki/Guides/AssetEditor$", "guides/creating-mods")
	.AddRedirect("^guides/survival-editor", "guides/creating-spawnsets")
	.AddRedirect("^guides/asset-editor", "guides/creating-mods")

	.Add(new CustomLeaderboardPageRewriteRules())
	.Add(new ModPageRewriteRules())
	.Add(new PlayerPageRewriteRules())
	.Add(new SpawnsetPageRewriteRules());

app.UseRewriter(options);

if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

if (!app.Environment.IsDevelopment())
	app.UseSentryTracing();

app.UseCors(defaultCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.UseOpenApi();
app.UseSwaggerUi();

if (!app.Environment.IsDevelopment())
{
#if ROLES
	CreateRolesIfNotExist(serviceProvider);
#endif
	StringBuilder sb = new();
	sb.Append("> **Application is now online in the `").Append(app.Environment.EnvironmentName).AppendLine("` environment.**");

	ILogContainerService lcs = app.Services.GetRequiredService<ILogContainerService>();
	lcs.AddLog($"{DateTime.UtcNow:HH:mm:ss.fff}: Starting...\n{sb}");
}

app.Run();

#if ROLES
private static void CreateRolesIfNotExist(IServiceProvider serviceProvider)
{
	using ApplicationDbContext dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
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
#endif
