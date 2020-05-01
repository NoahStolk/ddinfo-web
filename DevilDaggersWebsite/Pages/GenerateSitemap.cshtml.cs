using CoreBase3.Services;
using CoreBase3.Sitemap;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Pages
{
	public class GenerateSitemapModel : PageModel
	{
		private readonly ICommonObjects commonObjects;

		public string XmlResult { get; set; }

		public GenerateSitemapModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;
		}

		public void OnGet()
		{
			Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("Views"));
			foreach (Type type in asm.GetTypes().Where(t => t.Name.Contains("Admin") || t.Name.Contains("Api")))
				SitemapUtils.ExcludePage(type);

			SitemapUtils.ExcludePage("CustomLeaderboards_Leaderboard");
			SitemapUtils.ExcludePage("CustomLeaderboards_Upload");
			SitemapUtils.ExcludePage("Error_404");
			SitemapUtils.ExcludePage("Leaderboard_Statistics");
			SitemapUtils.ExcludePage("Spawnset");
			SitemapUtils.ExcludePage("Wiki_SpawnsExtended");

			XmlResult = SitemapUtils.GetSitemap(commonObjects, true);
		}
	}
}