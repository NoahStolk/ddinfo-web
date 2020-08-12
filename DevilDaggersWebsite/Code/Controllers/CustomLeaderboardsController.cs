using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/custom-leaderboards")]
	[ApiController]
	public class CustomLeaderboardsController : ControllerBase
	{
		private readonly ApplicationDbContext context;

		public CustomLeaderboardsController(ApplicationDbContext context)
		{
			this.context = context;
		}

		[HttpGet]
		[ProducesResponseType(200)]
		public ActionResult<List<CustomLeaderboardBase>> GetCustomLeaderboards()
			=> context.CustomLeaderboards.Select(cl => new CustomLeaderboardBase(cl.SpawnsetFileName, cl.Bronze, cl.Silver, cl.Golden, cl.Devil, cl.Homing, cl.DateLastPlayed, cl.DateCreated)).ToList();
	}
}