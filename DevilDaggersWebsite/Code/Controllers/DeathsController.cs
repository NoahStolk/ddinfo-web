using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/deaths")]
	[ApiController]
	public class DeathsController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(200)]
		public ActionResult<List<Death>> GetDeaths(GameVersion? gameVersion = null)
			=> GameInfo.GetEntities<Death>(gameVersion);

		[HttpGet("by-type")]
		[ProducesResponseType(200)]
		public ActionResult<Death> GetDeathByType([Required] int type, GameVersion? gameVersion = null)
			=> GameInfo.GetEntities<Death>(gameVersion).FirstOrDefault(d => d.DeathType == type) ?? GameData.V3Unknown;

		[HttpGet("by-name")]
		[ProducesResponseType(200)]
		public ActionResult<Death> GetDeathByName([Required] string name, GameVersion? gameVersion = null)
			=> GameInfo.GetEntities<Death>(gameVersion).FirstOrDefault(d => d.Name.ToLower() == name.ToLower()) ?? GameData.V3Unknown;
	}
}