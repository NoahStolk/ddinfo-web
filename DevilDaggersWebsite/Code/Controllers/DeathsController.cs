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
		public ActionResult<List<Death>> GetDeaths(GameVersion? gameVersion = GameVersion.V3)
			=> GameInfo.GetEntities<Death>(gameVersion);

		[HttpGet("{type}")]
		[ProducesResponseType(200)]
		public ActionResult<Death> GetDeathByType([Required] byte type, GameVersion? gameVersion = GameVersion.V3)
			=> GameInfo.GetEntities<Death>(gameVersion).FirstOrDefault(d => d.DeathType == type) ?? GameData.V3Unknown;

		[HttpGet("{name}")]
		[ProducesResponseType(200)]
		public ActionResult<Death> GetDeathByName([Required] string name, GameVersion? gameVersion = GameVersion.V3)
			=> GameInfo.GetEntities<Death>(gameVersion).FirstOrDefault(d => d.Name.ToLower() == name.ToLower()) ?? GameData.V3Unknown;
	}
}