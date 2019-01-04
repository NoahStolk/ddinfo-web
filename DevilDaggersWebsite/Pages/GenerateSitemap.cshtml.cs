﻿using CoreBase.Services;
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
			SitemapUtils.ExcludePage("Admin_Index");
			SitemapUtils.ExcludePage("API_DownloadSpawnset");
			SitemapUtils.ExcludePage("API_GetDeaths");
			SitemapUtils.ExcludePage("API_GetGameVersions");
			SitemapUtils.ExcludePage("API_GetLeaderboard");
			SitemapUtils.ExcludePage("API_GetSpawnset");
			SitemapUtils.ExcludePage("API_GetSpawnsets");
			SitemapUtils.ExcludePage("API_GetToolVersions");
			SitemapUtils.ExcludePage("API_GetWorldRecords");
			SitemapUtils.ExcludePage("API_Index");
			SitemapUtils.ExcludePage("Error_404");
			SitemapUtils.ExcludePage("Leaderboard_Statistics");
			SitemapUtils.ExcludePage("Spawnset");
			XmlResult = SitemapUtils.GetSitemap(_commonObjects, true);
		}
	}
}