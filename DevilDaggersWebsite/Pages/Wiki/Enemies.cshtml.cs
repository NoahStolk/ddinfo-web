using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class EnemiesModel : PageModel
	{
		public string gameVersion;

		public GameVersion gameVersionObject;
		public List<Enemy> enemies;
		public List<Upgrade> upgrades;

		public List<SelectListItem> GameVersions { get; set; }

		public void OnGet(string gameVersion)
		{
			if (Game.TryGetGameVersionFromString(gameVersion, out gameVersionObject))
				this.gameVersion = gameVersion;
			else
				this.gameVersion = Game.DEFAULT_GAME_VERSION;

			enemies = Game.GetEntities<Enemy>(gameVersionObject);
			upgrades = Game.GetEntities<Upgrade>(gameVersionObject);

			GameVersions = new List<SelectListItem>();
			for (int i = 0; i < 3; i++)
			{
				string gameVersionString = $"V{i + 1}";
				GameVersions.Add(new SelectListItem(gameVersionString, gameVersionString));
			}
		}
	}
}