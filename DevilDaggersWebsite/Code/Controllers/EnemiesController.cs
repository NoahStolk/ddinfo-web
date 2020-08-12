using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/enemies")]
	[ApiController]
	public class EnemiesController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(200)]
		public ActionResult<List<Enemy>> GetEnemies([Required] string enemyName, GameVersion? gameVersion = GameVersion.V3)
		{
			List<Enemy> query = GameInfo.GetEntities<Enemy>(gameVersion);
			if (!string.IsNullOrEmpty(enemyName))
				query = query.Where(e => e.Name == enemyName).ToList();

			return query;
		}
	}
}