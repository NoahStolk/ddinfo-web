using DevilDaggersCore.Game;
using DevilDaggersWebsite.Razor.PageModels;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pages.Wiki
{
	public class DaggersModel : WikiPageModel
	{
		public DaggersModel()
			: base(skipV3Next: false)
		{
		}

		public List<Dagger> Daggers { get; private set; } = null!;

		public void OnGet(GameVersion gameVersion = GameVersion.V32)
		{
			SetGameVersion(gameVersion);

			Daggers = GameInfo.GetDaggers(gameVersion);
		}
	}
}
