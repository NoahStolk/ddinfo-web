using DevilDaggersCore.Game;
using DevilDaggersWebsite.Models.PageModels;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class UpgradesModel : WikiPageModel
	{
		public List<Upgrade> upgrades;

		public void OnGet(string gameVersion)
		{
			GetGameVersion(gameVersion);

			upgrades = Game.GetEntities<Upgrade>(gameVersionObject);
		}
	}
}