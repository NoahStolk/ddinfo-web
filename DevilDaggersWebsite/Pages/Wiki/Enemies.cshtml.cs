using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.PageModels;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class EnemiesModel : WikiPageModel
	{
		public List<string> TableOfContents { get; set; } = new List<string>
		{
			"Summary",
			"Details",
			"Damage stats",
			"Transmuted skulls"
		};

		public List<Enemy> enemies;
		public List<Upgrade> upgrades;

		public void OnGet(string gameVersion)
		{
			SetGameVersion(gameVersion);

			enemies = GameInfo.GetEntities<Enemy>(gameVersionObject);
			upgrades = GameInfo.GetEntities<Upgrade>(gameVersionObject);

			if (gameVersionObject == GameInfo.GameVersions["V3"])
				TableOfContents.Add("Homing daggers");
		}
	}
}