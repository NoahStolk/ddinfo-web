using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class SpawnsModel : WikiPageModel
	{
		private readonly IWebHostEnvironment env;

		public SpawnsModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public string SpawnsetPath { get; set; }
		public string EmergeEnemies { get; set; }

		public void OnGet(GameVersion gameVersion = GameVersion.V3)
		{
			SetGameVersion(gameVersion);

			SpawnsetPath = Path.Combine(env.WebRootPath, "spawnsets", $"{GameVersion}_Sorath");
			EmergeEnemies = GameVersion == GameVersion.V3 ? "Centipedes, Gigapedes, Ghostpedes, and Thorns" : "Centipedes and Gigapedes";
		}
	}
}