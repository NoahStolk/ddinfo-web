using DevilDaggersCore.Game;
using DevilDaggersWebsite.Razor.PageModels;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pages.Wiki
{
	public class EnemiesModel : WikiPageModel
	{
		public List<Enemy> Enemies { get; private set; } = null!;

		public List<Upgrade> Upgrades { get; private set; } = null!;

		public List<string> TableOfContents { get; } = new List<string>
		{
			"Summary",
			"Details",
			"Damage stats",
			"Transmuted skulls",
		};

		public void OnGet(GameVersion gameVersion = GameVersion.V3)
		{
			SetGameVersion(gameVersion);

			Enemies = GameInfo.GetEntities<Enemy>(GameVersion);
			Upgrades = GameInfo.GetEntities<Upgrade>(GameVersion);

			if (GameVersion == GameVersion.V3)
				TableOfContents.Add("Homing daggers");
		}
	}
}