using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SitemapGenerator;
using System;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Pages
{
	public class GenerateSitemapModel : PageModel
	{
		private readonly IHttpContextAccessor httpContextAccessor;

		public GenerateSitemapModel(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

		public string? XmlResult { get; set; }

		public void OnGet()
		{
			Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName?.Contains("Views", StringComparison.InvariantCulture) ?? false);
			foreach (Type type in asm.GetTypes().Where(t => t.Name.Contains("Admin", StringComparison.InvariantCulture) || t.Name.Contains("Identity", StringComparison.InvariantCulture) || t.Name.Contains("Api", StringComparison.InvariantCulture)))
				SitemapUtils.ExcludePage(type);

			SitemapUtils.ExcludePage("CustomLeaderboards_Leaderboard");
			SitemapUtils.ExcludePage("CustomLeaderboards_Upload");
			SitemapUtils.ExcludePage("Error_404");
			SitemapUtils.ExcludePage("Spawnset");
			SitemapUtils.ExcludePage("Wiki_SpawnsExtended");

			XmlResult = SitemapUtils.GetSitemap(httpContextAccessor, typeof(Page), true);
		}
	}
}