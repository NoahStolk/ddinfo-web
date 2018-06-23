using CoreBase.Extensions;
using CoreBase.Services;
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
			SitemapUtils.ExcludePage("Spawnset");
			XmlResult = SitemapUtils.GetSitemap(_commonObjects);
		}
	}
}