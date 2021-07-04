using DevilDaggersCore.Game;
using DevilDaggersWebsite.Api.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Api
{
	[Route("api/game-versions")]
	[ApiController]
	public class GameVersionsController : ControllerBase
	{
		[HttpGet("{gameVersion}/release-date")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<DateTime> GetGameVersionReleaseDate([Required] GameVersion gameVersion)
		{
			DateTime? releaseDate = GameInfo.GetReleaseDate(gameVersion);
			return releaseDate ?? (ActionResult<DateTime>)new BadRequestObjectResult(new ProblemDetails { Title = $"Incorrect parameter {nameof(gameVersion)} '{gameVersion}' specified." });
		}

		[HttpGet("at-date")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<GameVersion> GetGameVersionAtDate([Required] DateTime date)
		{
			GameVersion? gameVersion = GameInfo.GetGameVersionFromDate(date);
			return gameVersion ?? (ActionResult<GameVersion>)new BadRequestObjectResult(new ProblemDetails { Title = $"No game version found at date '{date}'." });
		}
	}
}
