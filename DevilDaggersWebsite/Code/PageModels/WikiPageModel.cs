using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.PageModels
{
	public abstract class WikiPageModel : PageModel
	{
		public string gameVersion;
		public GameVersion gameVersionObject;
		public List<SelectListItem> GameVersionListItems { get; private set; } = new List<SelectListItem>();

		protected void SetGameVersion(string gameVersion)
		{
			this.gameVersion = GameInfo.TryGetGameVersionFromString(gameVersion, out gameVersionObject) ? gameVersion : GameInfo.DefaultGameVersion;

			for (int i = 0; i < GameInfo.GameVersions.Count; i++)
			{
				string gameVersionString = $"V{i + 1}";
				GameVersionListItems.Add(new SelectListItem(gameVersionString, gameVersionString));
			}
		}
	}
}