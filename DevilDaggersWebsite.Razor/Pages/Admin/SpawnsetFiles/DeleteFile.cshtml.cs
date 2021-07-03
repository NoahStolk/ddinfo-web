using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Singletons;
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
		private readonly IWebHostEnvironment _environment;
		private readonly DiscordLogger _discordLogger;
		private readonly SpawnsetHashCache _spawnsetHashCache;

		public DeleteFileModel(IWebHostEnvironment environment, DiscordLogger discordLogger, SpawnsetHashCache spawnsetHashCache)
		{
			_environment = environment;
			_discordLogger = discordLogger;
			_spawnsetHashCache = spawnsetHashCache;
		}

		public IEnumerable<string> SpawnsetFileNames { get; private set; } = Enumerable.Empty<string>();

		public void OnGet()
		{
			SpawnsetFileNames = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "spawnsets")).Select(p => Path.GetFileName(p));
		}

		public async Task<ActionResult?> OnPost(string fileName)
		{
			string failedAttemptMessage = $":x: Failed attempt from `{GetIdentity()}` to delete SPAWNSET file";

			string path = Path.Combine(_environment.WebRootPath, "spawnsets", fileName);
			if (!System.IO.File.Exists(path))
			{
				await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $"{failedAttemptMessage}: File `{fileName}` does not exist.");
				return null;
			}

			System.IO.File.Delete(path);

			await _discordLogger.TryLog(Channel.MonitoringAuditLog, _environment.EnvironmentName, $":white_check_mark: `{GetIdentity()}` deleted SPAWNSET file :file_folder: `{fileName}`");

			_spawnsetHashCache.Clear();

			return RedirectToPage("Index");
		}
	}
}
