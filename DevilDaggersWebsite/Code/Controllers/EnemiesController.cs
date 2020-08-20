using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/enemies")]
	[ApiController]
	public class EnemiesController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Enemy>> GetEnemies(GameVersion? gameVersion = null, string? name = null, byte? spawnsetType = null)
		{
			IEnumerable<Enemy> query = GameInfo.GetEntities<Enemy>(gameVersion);

			if (!string.IsNullOrEmpty(name))
				query = query.Where(e => e.Name.ToLower(CultureInfo.InvariantCulture) == name.ToLower(CultureInfo.InvariantCulture));
			if (spawnsetType != null)
				query = query.Where(e => e.SpawnsetType == spawnsetType);

			return query.ToList();
		}
	}
}