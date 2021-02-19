using DevilDaggersCore.Game;
using DevilDaggersWebsite.Razor.PageModels;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pages.Wiki
{
	public class DaggersModel : WikiPageModel
	{
		public DaggersModel()
			: base(skipV31: false)
		{
		}

		public List<Dagger> Daggers { get; private set; } = null!;

		public void OnGet(GameVersion gameVersion = GameVersion.V31)
		{
			SetGameVersion(gameVersion);

			Daggers = GameInfo.GetEntities<Dagger>(GameVersion);
		}
	}
}
