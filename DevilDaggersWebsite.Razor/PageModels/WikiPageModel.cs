using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public abstract class WikiPageModel : PageModel
	{
		public GameVersion GameVersion { get; private set; }
		public List<SelectListItem> GameVersionListItems { get; } = new List<SelectListItem>();

		protected void SetGameVersion(GameVersion gameVersion = GameVersion.V3)
		{
			GameVersion = gameVersion;

			for (int i = 0; i < 3; i++)
			{
				string gameVersionString = $"V{i + 1}";
				GameVersionListItems.Add(new SelectListItem(gameVersionString, gameVersionString));
			}
		}
	}
}
