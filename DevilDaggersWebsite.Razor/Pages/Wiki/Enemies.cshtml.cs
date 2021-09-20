using DevilDaggersCore.Game;
using DevilDaggersWebsite.Razor.PageModels;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pages.Wiki
{
	public class EnemiesModel : WikiPageModel
	{
		public EnemiesModel()
			: base(skipV31: false)
		{
		}

		public List<Enemy> Enemies { get; private set; } = null!;

		public List<Upgrade> Upgrades { get; private set; } = null!;

		public List<string> TableOfContents { get; } = new()
		{
			"Summary",
			"Details",
			"Damage stats",
			"Transmuted skulls",
		};

		public void OnGet(GameVersion gameVersion = GameVersion.V31)
		{
			SetGameVersion(gameVersion);

			Enemies = GameInfo.GetEnemies(gameVersion);
			Upgrades = GameInfo.GetUpgrades(gameVersion);

			if (GameVersion >= GameVersion.V3)
				TableOfContents.Add("Homing daggers");
		}
	}
}
