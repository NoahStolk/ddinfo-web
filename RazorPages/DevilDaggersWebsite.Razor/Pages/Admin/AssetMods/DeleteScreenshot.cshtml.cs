using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class DeleteScreenshotModel : AbstractAdminPageModel
	{
		private readonly IWebHostEnvironment _environment;
		private readonly DiscordLogger _discordLogger;

		public DeleteScreenshotModel(IWebHostEnvironment environment, DiscordLogger discordLogger)
		{
			_environment = environment;
			_discordLogger = discordLogger;
		}

		public List<string> ModFileNames { get; } = new();

		public void OnGet()
		{
			foreach (string path in Directory.GetFiles(Path.Combine(_environment.WebRootPath, "mod-screenshots"), "*.png", SearchOption.AllDirectories))
			{
				string directoryName = new DirectoryInfo(path).Parent?.Name ?? throw new($"Invalid path `{path}` while scanning mod screenshot file sizes.");
				ModFileNames.Add(Path.Combine(directoryName, Path.GetFileName(path)));
			}
		}

		public async Task<ActionResult?> OnPost(string fileName)
		{
			string failedAttemptMessage = $":x: Failed attempt from `{GetIdentity()}` to delete ASSETMOD screenshot";

			string path = Path.Combine(_environment.WebRootPath, "mod-screenshots", fileName);
			if (!System.IO.File.Exists(path))
			{
				await _discordLogger.TryLog(Channel.MonitoringAuditLog, $"{failedAttemptMessage}: File `{fileName}` does not exist.");
				return null;
			}

			System.IO.File.Delete(path);

			await _discordLogger.TryLog(Channel.MonitoringAuditLog, $":white_check_mark: `{GetIdentity()}` deleted ASSETMOD screenshot :frame_photo: `{fileName}`.");

			return RedirectToPage("Index");
		}
	}
}
