using DevilDaggersCore.Game;
using DevilDaggersWebsite.Models.PageModels;
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

			enemies = Game.GetEntities<Enemy>(gameVersionObject);
			upgrades = Game.GetEntities<Upgrade>(gameVersionObject);

			if (gameVersionObject == Game.GameVersions["V3"])
				TableOfContents.Add("Homing daggers");
		}
	}
}