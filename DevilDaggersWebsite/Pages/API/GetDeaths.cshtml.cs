using DevilDaggersCore.Game;
using DevilDaggersWebsite.Models.API;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the Death object for the given deathType and gameVersion. Returns all Death objects if no deathType parameter was specified. Returns the unknown (N/A) Death object if an invalid deathType was specified. Returns the Death object found in V3 if no gameVersion parameter was specified or if the parameter was incorrect.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetDeathsModel : ApiPageModel
	{
		public FileResult OnGet(string deathType = null, string gameVersion = Game.DEFAULT_GAME_VERSION, bool formatted = false)
		{
			if (!Game.GameVersions.TryGetValue(gameVersion, out GameVersion version))
				version = Game.GameVersions[Game.DEFAULT_GAME_VERSION];

			if (!string.IsNullOrEmpty(deathType) && int.TryParse(deathType, out int type))
				return JsonFile(Game.GetDeathFromDeathType(type, version));

			return JsonFile(Game.GetEntities<Death>(version), formatted ? Formatting.Indented : Formatting.None);
		}
	}
}