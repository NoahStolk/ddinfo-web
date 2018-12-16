using DevilDaggersCore.Game;
using DevilDaggersWebsite.Models.PageModels;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class EnemiesModel : WikiPageModel
	{
		public List<Enemy> enemies;
		public List<Upgrade> upgrades;

		public void OnGet(string gameVersion)
		{
			GetGameVersion(gameVersion);

			enemies = Game.GetEntities<Enemy>(gameVersionObject);
			upgrades = Game.GetEntities<Upgrade>(gameVersionObject);
		}
	}
}