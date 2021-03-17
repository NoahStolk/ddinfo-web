using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Api
{
	[Route("api/deaths")]
	[ApiController]
	public class DeathsController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Death>> GetDeaths(GameVersion? gameVersion = null, string? name = null, byte? type = null)
		{
			IEnumerable<Death> deaths = GameInfo.GetDeaths(gameVersion ?? GameVersion.V31);

			if (!string.IsNullOrEmpty(name))
				deaths = deaths.Where(d => string.Equals(d.Name, name));
			if (type != null)
				deaths = deaths.Where(d => d.DeathType == type);

			return deaths.ToList();
		}
	}
}
