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
		public ActionResult<List<Enemy>> GetEnemies(GameVersion? gameVersion = null)
			=> GameInfo.GetEntities<Enemy>(gameVersion);

		[HttpGet("by-name")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public ActionResult<List<Enemy>> GetEnemiesByName([Required] string enemyName, GameVersion? gameVersion = null)
		{
			IEnumerable<Enemy> enemies = GameInfo.GetEntities<Enemy>(gameVersion).Where(e => e.Name == enemyName);
			if (!enemies.Any())
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Enemy '{enemyName}' was not found." });
			return enemies.ToList();
		}

		[HttpGet("by-type")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public ActionResult<List<Enemy>> GetEnemyBySpawnsetType([Required] byte spawnsetType, GameVersion? gameVersion = null)
		{
			IEnumerable<Enemy> enemies = GameInfo.GetEntities<Enemy>(gameVersion).Where(e => e.SpawnsetType == spawnsetType);
			if (!enemies.Any())
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Enemy with type '{spawnsetType}' was not found." });
			return enemies.ToList();
		}
	}
}