using DevilDaggersCore.Game;
using DevilDaggersWebsite.Api.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Api
{
	[Route("api/enemies")]
	[ApiController]
	public class EnemiesController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<List<Enemy>> GetEnemies(GameVersion? gameVersion = null, string? name = null, byte? spawnsetType = null)
		{
			IEnumerable<Enemy> enemies = GameInfo.GetEnemies(gameVersion ?? GameVersion.V31);

			if (!string.IsNullOrEmpty(name))
				enemies = enemies.Where(e => string.Equals(e.Name, name));
			if (spawnsetType != null)
				enemies = enemies.Where(e => e.SpawnsetType == spawnsetType);

			return enemies.ToList();
		}
	}
}
