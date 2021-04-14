using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Caches;
using DevilDaggersWebsite.Razor.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.SpawnsetFiles
{
	public class DeleteFileModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public DeleteFileModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		public IEnumerable<string> SpawnsetFileNames { get; private set; } = Enumerable.Empty<string>();

		public void OnGet()
		{
			SpawnsetFileNames = Directory.GetFiles(Path.Combine(_env.WebRootPath, "spawnsets")).Select(p => Path.GetFileName(p));
		}

		public async Task<ActionResult> OnPost(string fileName)
		{
			System.IO.File.Delete(Path.Combine(_env.WebRootPath, "spawnsets", fileName));

			await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"`{this.GetIdentity()}` deleted SPAWNSET file `{fileName}`");

			await SpawnsetHashCache.Instance.Clear(_env);

			return RedirectToPage("Index");
		}
	}
}
