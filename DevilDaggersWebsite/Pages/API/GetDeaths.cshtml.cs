using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Models.Game;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the Death object for the given Death Type. Returns all Death objects if no Death Type parameter was specified.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetDeathsModel : ApiPageModel
	{
		public FileResult OnGet(string deathType = null)
		{
			if (!string.IsNullOrEmpty(deathType) && int.TryParse(deathType, out int type))
				return JsonFile(GetDeathFromDeathType(type));

			List<Death> deaths = new List<Death> { GameUtils.Unknown };
			deaths.AddRange(GameUtils.Deaths);
			return JsonFile(deaths);
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