using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Razor.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.SpawnsetFiles
{
	public class DeleteFileModel : AbstractAdminPageModel
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
			string failedAttemptMessage = $":x: Failed attempt from `{GetIdentity()}` to delete SPAWNSET file";

			string path = Path.Combine(_env.WebRootPath, "spawnsets", fileName);
			if (!System.IO.File.Exists(path))
			{
				await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $"{failedAttemptMessage}: File `{fileName}` does not exist.");
				return null;
			}

			System.IO.File.Delete(path);

			await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, $":white_check_mark: `{GetIdentity()}` deleted SPAWNSET file :file_folder: `{fileName}`");

			SpawnsetHashCache.Instance.Clear();

			return RedirectToPage("Index");
		}
	}
}
