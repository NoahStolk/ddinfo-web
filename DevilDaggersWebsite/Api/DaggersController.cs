using DevilDaggersCore.Game;
using DevilDaggersWebsite.Api.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Api
{
	[Route("api/daggers")]
	[ApiController]
	public class DaggersController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<List<Dagger>> GetDaggers()
			=> GameInfo.GetDaggers(GameVersion.V31);

		[HttpGet("at-time")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<Dagger> GetDaggerAtSeconds([Required] uint seconds)
			=> GameInfo.GetDaggerFromSeconds(GameVersion.V31, seconds);
	}
}
