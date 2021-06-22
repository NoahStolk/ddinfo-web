using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.PageModels;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages.Admin.SpawnsetFiles
{
	public class IndexModel : AdminEntityIndexPageModel<SpawnsetFile>
	{
		public IndexModel(ApplicationDbContext dbContext, IWebHostEnvironment environment)
			: base(dbContext, environment)
		{
		}

		public List<string> DeadFiles { get; } = new();
		public List<string> MissingFiles { get; } = new();

		public void OnGet()
		{
			List<string> allDbSpawnsetFileNames = DbContext.SpawnsetFiles.Select(sf => sf.Name).ToList();

			foreach (string path in Directory.GetFiles(Path.Combine(Environment.WebRootPath, "spawnsets")))
			{
				string fileName = Path.GetFileName(path);
				if (!allDbSpawnsetFileNames.Contains(fileName))
					DeadFiles.Add(fileName);
			}

			foreach (string dbSpawnsetFileName in allDbSpawnsetFileNames)
			{
				if (!System.IO.File.Exists(Path.Combine(Environment.WebRootPath, "spawnsets", dbSpawnsetFileName)))
					MissingFiles.Add(dbSpawnsetFileName);
			}
		}
	}
}
