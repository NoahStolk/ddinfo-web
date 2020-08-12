using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Code.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class GamesController : ControllerBase
	{
		[HttpGet("deaths")]
		[ProducesResponseType(200)]
		public ActionResult<List<Death>> GetDeaths([FromQuery] GameVersion gameVersion)
			=> GameInfo.GetEntities<Death>(gameVersion);

		[HttpGet("death/by-type")]
		[ProducesResponseType(200)]
		public ActionResult<Death> GetDeathByType([FromQuery] byte deathType, [FromQuery] GameVersion gameVersion)
			=> GameInfo.GetEntities<Death>(gameVersion).FirstOrDefault(d => d.DeathType == deathType) ?? GameData.V3Unknown;

		[HttpGet("death/by-name")]
		[ProducesResponseType(200)]
		public ActionResult<Death> GetDeathByName([FromQuery] string deathName, [FromQuery] GameVersion gameVersion)
			=> GameInfo.GetEntities<Death>(gameVersion).FirstOrDefault(d => d.Name.ToLower() == deathName.ToLower()) ?? GameData.V3Unknown;

		[HttpGet("version/release-date")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public ActionResult<DateTime> GetGameVersionReleaseDate([FromQuery] GameVersion gameVersion)
		{
			DateTime? releaseDate = GameInfo.GetReleaseDate(gameVersion);
			if (releaseDate == null)
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Incorrect parameter {nameof(gameVersion)} '{gameVersion}' specified." });
			return releaseDate;
		}

		[HttpGet("dagger/at-time")]
		[ProducesResponseType(200)]
		public ActionResult<Dagger> GetDaggerFromTimeInTenthsOfMilliseconds([FromQuery] int timeInTenthsOfMilliseconds)
			=> GameInfo.GetDaggerFromTime(timeInTenthsOfMilliseconds);

		[HttpGet("enemies")]
		[ProducesResponseType(200)]
		public ActionResult<List<Enemy>> GetEnemies(string enemyName, GameVersion gameVersion)
		{
			List<Enemy> query = GameInfo.GetEntities<Enemy>(gameVersion);
			if (!string.IsNullOrEmpty(enemyName))
				query = query.Where(e => e.Name == enemyName).ToList();

			return query;
		}
	}
}