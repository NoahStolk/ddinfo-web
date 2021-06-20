using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Caches.SpawnsetHash;
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

		public async Task<ActionResult?> OnPost(string fileName)
		{
			string failedAttemptMessage = $":x: Failed attempt from `{this.GetIdentity()}` to delete SPAWNSET file";

			string path = Path.Combine(_env.WebRootPath, "spawnsets", fileName);
			if (!System.IO.File.Exists(path))
			{
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File `{fileName}` does not exist.");
				return null;
			}

			System.IO.File.Delete(path);

			await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $":white_check_mark: `{this.GetIdentity()}` deleted SPAWNSET file :file_folder: `{fileName}`");

			await SpawnsetHashCache.Instance.Clear(_env);

			return RedirectToPage("Index");
		}
	}
}
