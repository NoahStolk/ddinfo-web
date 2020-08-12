using DevilDaggersCore.Game;
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
		[ProducesResponseType(200)]
		public ActionResult<List<Dagger>> GetDaggers()
			=> GameInfo.GetEntities<Dagger>(GameVersion.V3);

		[HttpGet("{atSeconds}")]
		[ProducesResponseType(200)]
		public ActionResult<Dagger> GetDaggerAtSeconds([Required] uint atSeconds)
			=> GameInfo.GetDaggerFromTime((int)atSeconds * 10000);
	}
}