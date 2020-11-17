using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.PageModels;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class UpgradesModel : WikiPageModel
	{
		public List<Upgrade> Upgrades { get; private set; } = null!;

		public void OnGet(GameVersion gameVersion = GameVersion.V3)
		{
			SetGameVersion(gameVersion);

			Upgrades = GameInfo.GetEntities<Upgrade>(GameVersion);
		}
	}
}