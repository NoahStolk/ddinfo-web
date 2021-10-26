using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public abstract class WikiPageModel : PageModel
	{
		private readonly bool _skipV3Next;

		protected WikiPageModel(bool skipV3Next)
		{
			_skipV3Next = skipV3Next;
		}

		public GameVersion GameVersion { get; private set; }
		public List<SelectListItem> GameVersionListItems { get; } = new();

		protected void SetGameVersion(GameVersion gameVersion = GameVersion.V32)
		{
			if (_skipV3Next && gameVersion > GameVersion.V3)
				gameVersion = GameVersion.V3;

			GameVersion = gameVersion;

			GameVersionListItems.Clear();
			foreach (GameVersion e in (GameVersion[])Enum.GetValues(typeof(GameVersion)))
			{
				if (_skipV3Next && e is GameVersion.V31 or GameVersion.V32)
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

			if (gameVersion == GameVersion.V32)
				return "V3.2";

			return gameVersion.ToString();
		}
	}
}
