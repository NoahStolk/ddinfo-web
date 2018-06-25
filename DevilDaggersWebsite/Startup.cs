using CoreBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

			AddCommonCoreBaseServices(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			RewriteOptions options = new RewriteOptions()
				.AddRedirect("Home/Index", "Index")
				.AddRedirect("Home/Leaderboard", "Leaderboard")
				.AddRedirect("Home/Spawnset/(.*)", "Spawnset?spawnset=$1")
				.AddRedirect("Home/Spawnsets", "Spawnsets")
				.AddRedirect("Home/SpawnsetsInfo", "Spawnsets")
				.AddRedirect("Home/Spawnset", "Spawnset")
				.AddRedirect("Home/Spawns", "Spawns")
				.AddRedirect("Home/Hands", "Upgrades")
				.AddRedirect("Home/Enemies", "Enemies")
				.AddRedirect("Home/Daggers", "Daggers")
				.AddRedirect("Home/Donations", "Donations")
				.AddRedirect("Home", "Index");

			app.UseRewriter(options);

			ConfigureErrorPageAndSSL(app, env);

			app.UseStaticFiles();

			app.UseMvc();

			app.UseStatusCodePagesWithRedirects("/Error/{0}");
		}
	}
}