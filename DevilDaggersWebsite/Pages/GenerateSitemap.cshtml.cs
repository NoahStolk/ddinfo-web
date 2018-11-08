using CoreBase.Services;
using CoreBase.Sitemap;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevilDaggersWebsite.Pages
{
	public class GenerateSitemapModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public string XmlResult { get; set; }

		public GenerateSitemapModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet()
		{
			SitemapUtils.ExcludePage("DownloadSpawnset");
			SitemapUtils.ExcludePage("GetSpawnsets");
			SitemapUtils.ExcludePage("GetToolVersions");
			SitemapUtils.ExcludePage("LeaderboardJson");
			SitemapUtils.ExcludePage("Spawnset");
			XmlResult = SitemapUtils.GetSitemap(_commonObjects);
		}
	}
}