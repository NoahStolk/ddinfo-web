using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Models.PageModels
{
	public abstract class WikiPageModel : PageModel
	{
		public string gameVersion;
		public GameVersion gameVersionObject;
		public List<SelectListItem> GameVersions { get; set; }

		protected void GetGameVersion(string gameVersion)
		{
			if (Game.TryGetGameVersionFromString(gameVersion, out gameVersionObject))
				this.gameVersion = gameVersion;
			else
				this.gameVersion = Game.DEFAULT_GAME_VERSION;

			GameVersions = new List<SelectListItem>();
			for (int i = 0; i < 3; i++)
			{
				string gameVersionString = $"V{i + 1}";
				GameVersions.Add(new SelectListItem(gameVersionString, gameVersionString));
			}
		}
	}
}