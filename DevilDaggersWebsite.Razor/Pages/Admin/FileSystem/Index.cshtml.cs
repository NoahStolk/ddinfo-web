using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages.Admin.FileSystem
{
	public class IndexModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public IndexModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		public Dictionary<string, long> TotalSpaceInBytesPerDrive { get; private set; } = new();
		public Dictionary<string, long> ModFileSizes { get; private set; } = new();
		public Dictionary<string, long> ModScreenshotFileSizes { get; private set; } = new();
		public Dictionary<string, long> SpawnsetFileSizes { get; private set; } = new();
		public Dictionary<string, long> LeaderboardHistoryFileSizes { get; private set; } = new();

		public void OnGet()
		{
			foreach (DriveInfo di in DriveInfo.GetDrives())
			{
				if (di.IsReady)
					TotalSpaceInBytesPerDrive.Add(di.Name, di.TotalFreeSpace);
			}

			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "mods")))
				ModFileSizes.Add(Path.GetFileName(path), new FileInfo(path).Length);

			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "mod-screenshots"), "*.png", SearchOption.AllDirectories))
			{
				string directoryName = new DirectoryInfo(path).Parent?.Name ?? throw new($"Invalid path '{path}' while scanning mod screenshot file sizes.");
				ModScreenshotFileSizes.Add(Path.Combine(directoryName, Path.GetFileName(path)), new FileInfo(path).Length);
			}

			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "spawnsets")))
				SpawnsetFileSizes.Add(Path.GetFileName(path), new FileInfo(path).Length);

			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "leaderboard-history")))
				LeaderboardHistoryFileSizes.Add(Path.GetFileName(path), new FileInfo(path).Length);

			TotalSpaceInBytesPerDrive = TotalSpaceInBytesPerDrive.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			ModFileSizes = ModFileSizes.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			ModScreenshotFileSizes = ModScreenshotFileSizes.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			SpawnsetFileSizes = SpawnsetFileSizes.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			LeaderboardHistoryFileSizes = LeaderboardHistoryFileSizes.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}
	}
}
