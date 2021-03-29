using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public abstract class WikiPageModel : PageModel
	{
		private readonly bool _skipV31;

		protected WikiPageModel(bool skipV31)
		{
			_skipV31 = skipV31;
		}

		public GameVersion GameVersion { get; private set; }
		public List<SelectListItem> GameVersionListItems { get; } = new();

		protected void SetGameVersion(GameVersion gameVersion = GameVersion.V31)
		{
			if (_skipV31 && gameVersion > GameVersion.V31)
				gameVersion = GameVersion.V3;

			GameVersion = gameVersion;

			GameVersionListItems.Clear();
			foreach (GameVersion e in (GameVersion[])Enum.GetValues(typeof(GameVersion)))
			{
				if (_skipV31 && e == GameVersion.V31)
					continue;

				string text = e.ToString();
				string value = GetGameVersionName(e);
				GameVersionListItems.Add(new(value, text));
			}
		}

		public string GetGameVersionName(GameVersion gameVersion)
		{
			if (gameVersion == GameVersion.V31)
				return "V3.1";

			return gameVersion.ToString();
		}
	}
}
