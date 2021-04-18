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
		private readonly IWebHostEnvironment _env;

		public IndexModel(ApplicationDbContext dbContext, IWebHostEnvironment env)
			: base(dbContext)
		{
			_env = env;
		}

		public List<string> DeadFiles { get; } = new();
		public List<string> MissingFiles { get; } = new();

		public void OnGet()
		{
			List<string> allDbSpawnsetFileNames = DbContext.SpawnsetFiles.Select(sf => sf.Name).ToList();

			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "spawnsets")))
			{
				string fileName = Path.GetFileName(path);
				if (!allDbSpawnsetFileNames.Contains(fileName))
					DeadFiles.Add(fileName);
			}

			foreach (string dbSpawnsetFileName in allDbSpawnsetFileNames)
			{
				if (!System.IO.File.Exists(Path.Combine(_env.WebRootPath, "spawnsets", dbSpawnsetFileName)))
					MissingFiles.Add(dbSpawnsetFileName);
			}
		}
	}
}
