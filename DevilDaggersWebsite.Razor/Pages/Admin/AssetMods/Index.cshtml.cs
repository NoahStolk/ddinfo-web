using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.PageModels;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class IndexModel : AdminEntityIndexPageModel<AssetMod>
	{
		public IndexModel(ApplicationDbContext dbContext, IWebHostEnvironment environment)
			: base(dbContext, environment)
		{
		}

		public List<string> DeadFiles { get; } = new();
		public List<string> DeadScreenshots { get; } = new();

		public void OnGet()
		{
			foreach (string path in Directory.GetFiles(Path.Combine(Environment.WebRootPath, "mods")))
			{
				string fileName = Path.GetFileNameWithoutExtension(path);
				if (!DbContext.AssetMods.Any(am => am.Name == fileName))
					DeadFiles.Add(fileName);
			}

			List<string> directoriesScanned = new();
			foreach (string path in Directory.GetFiles(Path.Combine(Environment.WebRootPath, "mod-screenshots"), "*.png", SearchOption.AllDirectories))
			{
				string directoryName = new DirectoryInfo(path).Parent?.Name ?? throw new($"Invalid path '{path}' while scanning for dead mod screenshots.");

				if (directoriesScanned.Contains(directoryName))
					continue;

				if (!DbContext.AssetMods.Any(am => am.Name == directoryName))
					DeadScreenshots.Add(Path.Combine(directoryName, Path.GetFileName(path)));

				directoriesScanned.Add(directoryName);
			}
		}
	}
}
