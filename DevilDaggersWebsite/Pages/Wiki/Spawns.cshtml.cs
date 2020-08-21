using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.PageModels;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class SpawnsModel : WikiPageModel
	{
		public string SpawnsetPath { get; set; }
		public string EmergeEnemies { get; set; }

		public void OnGet(GameVersion gameVersion = GameVersion.V3)
		{
			SetGameVersion(gameVersion);

			SpawnsetPath = GameVersion.ToString();
			EmergeEnemies = GameVersion == GameVersion.V3 ? "Centipedes, Gigapedes, Ghostpedes, and Thorns" : "Centipedes and Gigapedes";
		}
	}
}