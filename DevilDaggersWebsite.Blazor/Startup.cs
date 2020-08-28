using DevilDaggersWebsite.Blazor.Areas.Identity;
using DevilDaggersWebsite.Core.Authorization;
using DevilDaggersWebsite.Core.Entities;
using DevilDaggersWebsite.Core.Tasks;
using DevilDaggersWebsite.Core.Tasks.Scheduling;
using DevilDaggersWebsite.Core.Transients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

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
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

			services.AddCors(options =>
			{
				options.AddPolicy(_defaultCorsPolicy, builder => { builder.AllowAnyOrigin(); });
			});

			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

			services.AddMvc();

			services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
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

			services.AddScoped<IUrlHelper>(factory => new UrlHelper(factory.GetService<IActionContextAccessor>().ActionContext));

			services.AddTransient<LeaderboardHistoryHelper>();
			services.AddTransient<SpawnsetHelper>();

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
				foreach (KeyValuePair<string, string> kvp in RoleManager.PolicyToRoleMapper)
					options.AddPolicy(kvp.Key, policy => policy.RequireRole(kvp.Value));
			});

			services.AddRazorPages().AddRazorPagesOptions(options =>
			{
				foreach (KeyValuePair<string, string> kvp in RoleManager.FolderToPolicyMapper)
					options.Conventions.AuthorizeFolder(kvp.Key, kvp.Value);
			});

			services.AddSwaggerDocument(config =>
			{
				config.PostProcess = document =>
				{
					document.Info.Title = "DevilDaggers.Info API";
				};
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
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
		}
	}
}