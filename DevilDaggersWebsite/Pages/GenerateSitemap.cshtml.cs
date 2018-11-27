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
			SitemapUtils.ExcludePage("API_GetSpawnsets");
			SitemapUtils.ExcludePage("API_GetToolVersions");
			SitemapUtils.ExcludePage("API_LeaderboardJson");
			SitemapUtils.ExcludePage("API_DownloadSpawnset");
			SitemapUtils.ExcludePage("Error_404");
			SitemapUtils.ExcludePage("LeaderboardHistory");
			SitemapUtils.ExcludePage("Spawnset");
			XmlResult = SitemapUtils.GetSitemap(_commonObjects, true);
		}
	}
}