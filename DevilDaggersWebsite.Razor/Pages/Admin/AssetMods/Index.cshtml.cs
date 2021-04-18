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
		private readonly IWebHostEnvironment _env;

		public IndexModel(ApplicationDbContext dbContext, IWebHostEnvironment env)
			: base(dbContext)
		{
			_env = env;
		}

		public List<string> DeadFiles { get; } = new();

		public void OnGet()
		{
			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "mods")))
			{
				string fileName = Path.GetFileNameWithoutExtension(path);
				if (!DbContext.AssetMods.Any(am => am.Name == fileName))
					DeadFiles.Add(fileName);
			}
		}
	}
}
