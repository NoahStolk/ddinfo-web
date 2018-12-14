using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class SpawnsModel : PageModel
	{
		public string SpawnsetPath { get; set; }
		public GameVersion gameVersion;
		public string EmergeEnemies { get; set; }

		private readonly IHostingEnvironment _env;

		public SpawnsModel(IHostingEnvironment env)
		{
			_env = env;
		}

		public void OnGet()
		{
			string gameVersionQuery = HttpContext.Request.Query["gameVersion"];
			if (Game.TryGetGameVersionFromString(gameVersionQuery, out gameVersion))
				SpawnsetPath = Path.Combine(_env.WebRootPath, "spawnsets", $"{gameVersionQuery}_Sorath");
			else
				SpawnsetPath = Path.Combine(_env.WebRootPath, "spawnsets", $"V3_Sorath");

			EmergeEnemies = gameVersion == Game.GameVersions["V3"] ? "Centipedes, Gigapedes, Ghostpedes, and Thorns" : "Centipedes and Gigapedes";
		}
	}
}