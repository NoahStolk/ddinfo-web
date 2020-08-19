using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/deaths")]
	[ApiController]
	public class DeathsController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Death>> GetDeaths(GameVersion? gameVersion = null)
			=> GameInfo.GetEntities<Death>(gameVersion);

		[HttpGet("by-type")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<List<Death>> GetDeathsByType([Required] int type, GameVersion? gameVersion = null)
			=> GameInfo.GetEntities<Death>(gameVersion).Where(d => d.DeathType == type).ToList();

		[HttpGet("by-name")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<List<Death>> GetDeathsByName([Required] string name, GameVersion? gameVersion = null)
			=> GameInfo.GetEntities<Death>(gameVersion).Where(d => d.Name.ToLower(CultureInfo.InvariantCulture) == name.ToLower(CultureInfo.InvariantCulture)).ToList();
	}
}