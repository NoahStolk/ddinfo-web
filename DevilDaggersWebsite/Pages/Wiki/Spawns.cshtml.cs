using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class SpawnsModel : WikiPageModel
	{
		public string SpawnsetPath { get; set; }
		public string EmergeEnemies { get; set; }

		private readonly IWebHostEnvironment env;

		public SpawnsModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public void OnGet(string gameVersion)
		{
			SetGameVersion(gameVersion);

			SpawnsetPath = Path.Combine(env.WebRootPath, "spawnsets", $"{this.gameVersion}_Sorath");
			EmergeEnemies = this.gameVersion == "V3" ? "Centipedes, Gigapedes, Ghostpedes, and Thorns" : "Centipedes and Gigapedes";
		}
	}
}