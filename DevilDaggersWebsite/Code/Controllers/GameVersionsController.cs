using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/game-versions")]
	[ApiController]
	public class GameVersionsController : ControllerBase
	{
		[HttpGet("{gameVersion}/release-date")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public ActionResult<DateTime> GetGameVersionReleaseDate([Required] GameVersion gameVersion)
		{
			DateTime? releaseDate = GameInfo.GetReleaseDate(gameVersion);
			if (releaseDate == null)
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Incorrect parameter {nameof(gameVersion)} '{gameVersion}' specified." });
			return releaseDate;
		}

		[HttpGet("{atDate}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public ActionResult<GameVersion> GetGameVersionAtDate([Required] DateTime atDate)
		{
			GameVersion? gameVersion = GameInfo.GetGameVersionFromDate(atDate);
			if (gameVersion == null)
				return new BadRequestObjectResult(new ProblemDetails { Title = $"No game version found at date '{atDate}'." });
			return gameVersion;
		}
	}
}