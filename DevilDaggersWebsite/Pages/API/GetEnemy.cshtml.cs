using DevilDaggersCore.Game;
using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the Enemy object for the given enemyName and gameVersion. Returns the Enemy object found in V3 if no gameVersion parameter was specified or if the parameter was incorrect. Returns to this page if the Enemy object could not be found.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetEnemyModel : ApiPageModel
	{
		public ActionResult OnGet(string enemyName, string gameVersion = Game.DEFAULT_GAME_VERSION, bool formatted = false)
		{
			if (string.IsNullOrEmpty(enemyName))
				return RedirectToPage("/API/Index");

			if (!Game.GameVersions.TryGetValue(gameVersion, out GameVersion version))
				version = Game.GameVersions[Game.DEFAULT_GAME_VERSION];

			Enemy enemy = Game.GetEntities<Enemy>(version).Where(e => e.Name == enemyName).FirstOrDefault();
			if (enemy == null)
				return RedirectToPage("/API/Index");
			
			return JsonFile(enemy, formatted ? Formatting.Indented : Formatting.None);
		}
	}
}