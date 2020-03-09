using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the Enemy object for the given enemyName and gameVersion. Returns all Enemy objects if no enemyName parameter was specified. Returns null if the Enemy object could not be found. Returns the Enemy object found in V3 if no gameVersion parameter was specified or if the parameter was incorrect.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetEnemiesModel : ApiPageModel
	{
		public ActionResult OnGet(string enemyName = "", string gameVersion = GameInfo.DEFAULT_GAME_VERSION, bool formatted = false) => JsonFile(ApiFunctions.GetEnemies(enemyName, gameVersion), formatted ? Formatting.Indented : Formatting.None);
	}
}