using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the Enemy object for the given enemyName and gameVersion. Returns all Enemy objects if no enemyName parameter was specified. Returns null if the Enemy object could not be found. Returns the Enemy object found in V3 if no gameVersion parameter was specified or if the parameter was incorrect.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetEnemiesModel : ApiPageModel
	{
		public ActionResult OnGet(string enemyName = "", string gameVersion = GameInfo.DEFAULT_GAME_VERSION, bool formatted = false)
		{
			if (!GameInfo.GameVersions.TryGetValue(gameVersion, out GameVersion version))
				version = GameInfo.GameVersions[GameInfo.DEFAULT_GAME_VERSION];

			return JsonFile(!string.IsNullOrEmpty(enemyName) ? new List<Enemy> { GameInfo.GetEntities<Enemy>(version).Where(e => e.Name == enemyName).FirstOrDefault() } : GameInfo.GetEntities<Enemy>(version), formatted ? Formatting.Indented : Formatting.None);
		}
	}
}