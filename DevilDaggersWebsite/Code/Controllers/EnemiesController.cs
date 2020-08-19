using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Http;
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Enemy>> GetEnemies(GameVersion? gameVersion = null)
			=> GameInfo.GetEntities<Enemy>(gameVersion);

		[HttpGet("by-name")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<List<Enemy>> GetEnemiesByName([Required] string enemyName, GameVersion? gameVersion = null)
		{
			IEnumerable<Enemy> enemies = GameInfo.GetEntities<Enemy>(gameVersion).Where(e => e.Name == enemyName);
			if (!enemies.Any())
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Enemy '{enemyName}' was not found." });
			return enemies.ToList();
		}

		[HttpGet("by-type")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<List<Enemy>> GetEnemyBySpawnsetType([Required] byte spawnsetType, GameVersion? gameVersion = null)
		{
			IEnumerable<Enemy> enemies = GameInfo.GetEntities<Enemy>(gameVersion).Where(e => e.SpawnsetType == spawnsetType);
			if (!enemies.Any())
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Enemy with type '{spawnsetType}' was not found." });
			return enemies.ToList();
		}
	}
}