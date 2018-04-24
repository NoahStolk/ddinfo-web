using CoreBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

			base.AddCommonCoreBaseServices(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			base.ConfigureErrorPageAndSSL(app, env);

			app.UseStaticFiles();

			app.UseMvc();
		}
	}
}