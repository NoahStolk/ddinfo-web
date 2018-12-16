using DevilDaggersWebsite.Models.PageModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class SpawnsModel : WikiPageModel
	{
		public string SpawnsetPath { get; set; }
		public string EmergeEnemies { get; set; }

		private readonly IHostingEnvironment _env;

		public SpawnsModel(IHostingEnvironment env)
		{
			_env = env;
		}

		public void OnGet(string gameVersion)
		{
			GetGameVersion(gameVersion);

			SpawnsetPath = Path.Combine(_env.WebRootPath, "spawnsets", $"{this.gameVersion}_Sorath");
			EmergeEnemies = this.gameVersion == "V3" ? "Centipedes, Gigapedes, Ghostpedes, and Thorns" : "Centipedes and Gigapedes";
		}
	}
}