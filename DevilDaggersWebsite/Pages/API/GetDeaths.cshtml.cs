using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the Death object for the given deathType and gameVersion. Returns all Death objects if no deathType parameter was specified. Returns the unknown (N/A) Death object if an invalid deathType was specified. Returns the Death object found in V3 if no gameVersion parameter was specified or if the gameVersion parameter was incorrect.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetDeathsModel : ApiPageModel
	{
		public FileResult OnGet(int? deathType = null, string gameVersion = GameInfo.DEFAULT_GAME_VERSION, bool formatted = false) => JsonFile(ApiFunctions.GetDeaths(deathType, gameVersion), formatted ? Formatting.Indented : Formatting.None);
	}
}