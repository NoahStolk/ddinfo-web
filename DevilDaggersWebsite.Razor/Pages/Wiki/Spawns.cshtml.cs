using DevilDaggersCore.Game;
using DevilDaggersWebsite.Razor.PageModels;

namespace DevilDaggersWebsite.Razor.Pages.Wiki
{
	public class SpawnsModel : WikiPageModel
	{
		public string SpawnsetPath { get; private set; } = null!;
		public string EmergeEnemies { get; private set; } = null!;

		public void OnGet(GameVersion gameVersion = GameVersion.V3)
		{
			SetGameVersion(gameVersion);

			SpawnsetPath = GameVersion.ToString();
			EmergeEnemies = GameVersion == GameVersion.V3 ? "Centipedes, Gigapedes, Ghostpedes, and Thorns" : "Centipedes and Gigapedes";
		}
	}
}
