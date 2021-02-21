using DevilDaggersWebsite.SitemapGenerator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class GenerateSitemapModel : PageModel
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public GenerateSitemapModel(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string? XmlResult { get; set; }

		public void OnGet()
		{
			Assembly asm = Array.Find(AppDomain.CurrentDomain.GetAssemblies(), a => a.FullName?.Contains("Views", StringComparison.InvariantCulture) ?? false) ?? throw new("Could not retrieve 'Views' assembly.");
			foreach (Type type in asm.GetTypes().Where(t => t.Name.Contains("Admin", StringComparison.InvariantCulture) || t.Name.Contains("Identity", StringComparison.InvariantCulture) || t.Name.Contains("Api", StringComparison.InvariantCulture)))
				SitemapUtils.ExcludePage(type);

			SitemapUtils.ExcludePage("CustomLeaderboards_Leaderboard");
			SitemapUtils.ExcludePage("CustomLeaderboards_Upload");
			SitemapUtils.ExcludePage("Error_404");
			SitemapUtils.ExcludePage("Spawnset");
			SitemapUtils.ExcludePage("Wiki_SpawnsExtended");

			XmlResult = SitemapUtils.GetSitemap(_httpContextAccessor, typeof(Page), true);
		}
	}
}
