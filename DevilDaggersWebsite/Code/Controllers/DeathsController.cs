using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
		public ActionResult<List<Death>> GetDeaths(GameVersion? gameVersion = null, string? name = null, byte? type = null)
		{
			IEnumerable<Death> query = GameInfo.GetEntities<Death>(gameVersion);

			if (!string.IsNullOrEmpty(name))
				query = query.Where(e => e.Name.ToLower(CultureInfo.InvariantCulture) == name.ToLower(CultureInfo.InvariantCulture));
			if (type != null)
				query = query.Where(e => e.DeathType == type);

			return query.ToList();
		}
	}
}