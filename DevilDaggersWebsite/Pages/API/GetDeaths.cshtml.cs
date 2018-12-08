using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Models.Game;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the Death object for the given deathType. Returns all Death objects if no deathType parameter was specified.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetDeathsModel : ApiPageModel
	{
		public FileResult OnGet(string deathType = null, bool formatted = false)
		{
			if (!string.IsNullOrEmpty(deathType) && int.TryParse(deathType, out int type))
				return JsonFile(GetDeathFromDeathType(type));

			List<Death> deaths = new List<Death> { GameUtils.Unknown };
			deaths.AddRange(GameUtils.Deaths);
			return JsonFile(deaths, formatted ? Formatting.Indented : Formatting.None);
		}

		public Death GetDeathFromDeathType(int deathType)
		{
			try
			{
				return GameUtils.Deaths[deathType];
			}
			catch
			{
				return GameUtils.Unknown;
			}
		}
	}
}