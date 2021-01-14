using DevilDaggersWebsite.Authorization;
using DevilDaggersWebsite.Blazor.Areas.Identity;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Tasks;
using DevilDaggersWebsite.Tasks.Scheduling;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Blazor
{
	public class Startup
	{
		private const string _defaultCorsPolicy = nameof(_defaultCorsPolicy);

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options => options.AddPolicy(_defaultCorsPolicy, builder => builder.AllowAnyOrigin()));

			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

			services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

			services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), MySqlServerVersion.LatestSupportedServerVersion, providerOptions => providerOptions.EnableRetryOnFailure()));
			services.AddDefaultIdentity<IdentityUser>(options =>
				{
					options.SignIn.RequireConfirmedAccount = true;
					options.User.RequireUniqueEmail = true;
				})
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

			services.AddSingleton<IScheduledTask, CreateLeaderboardHistoryFileTask>();

			services.AddTransient<LeaderboardHistoryHelper>();
			services.AddTransient<SpawnsetHelper>();
			services.AddTransient<ToolHelper>();

			services.AddScheduler((sender, args) =>
			{
				Console.Write(args.Exception?.Message);
				args.SetObserved();
			});

			services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

			services.AddAuthorization(options =>
			{
				foreach (KeyValuePair<string, string> kvp in RoleManager.PolicyToRoleMapper)
					options.AddPolicy(kvp.Key, policy => policy.RequireRole(kvp.Value));
			});

			services.AddRazorPages().AddRazorPagesOptions(options =>
			{
				foreach (KeyValuePair<string, string> kvp in RoleManager.FolderToPolicyMapper)
					options.Conventions.AuthorizeFolder(kvp.Key, kvp.Value);
			});

			services.AddSwaggerDocument(config => config.PostProcess = document => document.Info.Title = "DevilDaggers.Info API");
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
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
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});

			app.UseOpenApi();
			app.UseSwaggerUi3();

			Task roleTask = serviceProvider.CreateRolesAndAdminUser(Configuration.GetSection("AdminUser")["Email"]);
			roleTask.Wait();
		}
	}
}
