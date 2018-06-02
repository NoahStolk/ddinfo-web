using CoreBase.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevilDaggersWebsite.Pages
{
	public class GenerateSitemapModel : PageModel
    {
		private IHostingEnvironment _env;
		private IHttpContextAccessor _httpContextAccessor;

		public string XmlResult { get; set; }

		public GenerateSitemapModel(IHostingEnvironment env, IHttpContextAccessor httpContextAccessor)
		{
			_env = env;
			_httpContextAccessor = httpContextAccessor;
		}

		public void OnGet()
		{
			SitemapUtils.ExcludePage("DownloadSpawnset");
			XmlResult = SitemapUtils.GetSitemap(_httpContextAccessor, _env);
		}
	}
}