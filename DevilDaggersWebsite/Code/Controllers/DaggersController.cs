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

		[HttpGet("at-time")]
		[ProducesResponseType(200)]
		public ActionResult<Dagger> GetDaggerAtSeconds([Required] uint seconds)
			=> GameInfo.GetDaggerFromTime((int)seconds * 10000);
	}
}