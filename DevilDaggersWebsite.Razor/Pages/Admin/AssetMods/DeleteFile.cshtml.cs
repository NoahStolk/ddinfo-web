using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Razor.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class DeleteFileModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public DeleteFileModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		public IEnumerable<string> ModFileNames { get; private set; } = Enumerable.Empty<string>();

		public void OnGet()
		{
			ModFileNames = Directory.GetFiles(Path.Combine(_env.WebRootPath, "mods")).Select(p => Path.GetFileName(p));
		}

		public async Task<ActionResult?> OnPost(string fileName)
		{
			string failedAttemptMessage = $":x: Failed attempt from `{this.GetIdentity()}` to delete ASSETMOD file";

			string path = Path.Combine(_env.WebRootPath, "mods", fileName);
			if (!System.IO.File.Exists(path))
			{
				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"{failedAttemptMessage}: File '{fileName}' does not exist.");
				return null;
			}

			System.IO.File.Delete(path);

			await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $":white_check_mark: `{this.GetIdentity()}` deleted ASSETMOD file `{fileName}`");

			await ModArchiveCache.Instance.Clear(_env);

			return RedirectToPage("Index");
		}
	}
}
