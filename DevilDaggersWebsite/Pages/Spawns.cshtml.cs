using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsModel : PageModel
    {
		public string V3Path { get; set; }

		private readonly IHostingEnvironment _env;

		public SpawnsModel(IHostingEnvironment env)
		{
			_env = env;
		}

		public void OnGet()
        {
			string webRoot = _env.WebRootPath;

			V3Path = Path.Combine(webRoot, string.Format(@"spawnsets/V3_Sorath"));
		}
    }
}