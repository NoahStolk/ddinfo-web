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
			SitemapUtils.ExcludePage("Admin_BanInfo");
			SitemapUtils.ExcludePage("Admin_Index");
			SitemapUtils.ExcludePage("Admin_Tasks");
			SitemapUtils.ExcludePage("Admin_TestException");
			SitemapUtils.ExcludePage("API_GetCustomLeaderboards");
			SitemapUtils.ExcludePage("API_GetDeaths");
			SitemapUtils.ExcludePage("API_GetEnemies");
			SitemapUtils.ExcludePage("API_GetGameVersions");
			SitemapUtils.ExcludePage("API_GetLeaderboard");
			SitemapUtils.ExcludePage("API_GetSpawnset");
			SitemapUtils.ExcludePage("API_GetSpawnsets");
			SitemapUtils.ExcludePage("API_GetTool");
			SitemapUtils.ExcludePage("API_GetTools");
			SitemapUtils.ExcludePage("API_GetToolVersions");
			SitemapUtils.ExcludePage("API_GetUserByID");
			SitemapUtils.ExcludePage("API_GetUserByUsername");
			SitemapUtils.ExcludePage("API_GetUserProgressionByID");
			SitemapUtils.ExcludePage("API_GetWorldRecords");
			SitemapUtils.ExcludePage("API_Index");
			SitemapUtils.ExcludePage("CustomLeaderboards_Leaderboard");
			SitemapUtils.ExcludePage("CustomLeaderboards_Upload");
			SitemapUtils.ExcludePage("Error_404");
			SitemapUtils.ExcludePage("Leaderboard_IDSearch");
			SitemapUtils.ExcludePage("Leaderboard_Statistics");
			SitemapUtils.ExcludePage("Spawnset");
			SitemapUtils.ExcludePage("Wiki_SpawnsExtended");
			XmlResult = SitemapUtils.GetSitemap(_commonObjects, true);
		}
	}
}