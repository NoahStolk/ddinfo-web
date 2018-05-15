using CoreBase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

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
			string baseUrl = _httpContextAccessor.HttpContext.Request.Host.Host;

			SitemapBuilder sitemapBuilder = new SitemapBuilder();

			foreach (string filePath in Directory.GetFiles(Path.Combine(_env.ContentRootPath, "Pages"), "*.cshtml", SearchOption.TopDirectoryOnly))
			{
				string file = Path.GetFileNameWithoutExtension(filePath);
				if (file[0] != '_' && file != "GenerateSitemap" && file != "Error")
				{
					sitemapBuilder.AddUrl(string.Format("{0}/{1}", baseUrl, file));
				}
			}

			XmlResult = sitemapBuilder.ToString();
		}
	}
}