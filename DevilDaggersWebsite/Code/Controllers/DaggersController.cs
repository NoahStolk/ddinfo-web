using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/daggers")]
	[ApiController]
	public class DaggersController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Dagger>> GetDaggers()
			=> GameInfo.GetEntities<Dagger>(GameVersion.V3);

		[HttpGet("at-time")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<Dagger> GetDaggerAtSeconds([Required] uint seconds)
			=> GameInfo.GetDaggerFromTime((int)seconds * 10000);
	}
}