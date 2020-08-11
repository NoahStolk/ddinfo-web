using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.PageModels
{
	public abstract class WikiPageModel : PageModel
	{
		public GameVersion GameVersion { get; private set; }
		public List<SelectListItem> GameVersionListItems { get; private set; } = new List<SelectListItem>();

		protected void SetGameVersion(GameVersion gameVersion)
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