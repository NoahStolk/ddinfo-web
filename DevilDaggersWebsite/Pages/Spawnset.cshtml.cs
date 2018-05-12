using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetModel : PageModel
	{
		public IHostingEnvironment Env { get; private set; }

		public SpawnsetModel(IHostingEnvironment env)
		{
			Env = env;
		}

		public FileResult Download()
		{
			return File(Path.Combine(Env.WebRootPath, string.Format("spawnsets/{0}", HttpContext.Request.Query["spawnset"])), MediaTypeNames.Application.Octet, HttpContext.Request.Query["spawnset"]);
		}
	}
}