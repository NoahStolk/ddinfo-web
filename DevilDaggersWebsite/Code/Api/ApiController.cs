using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Api
{
	[Route("[controller]")]
	[ApiController]
	public class ApiController : ControllerBase
	{
		private readonly ApplicationDbContext context;

		public ApiController(ApplicationDbContext context)
		{
			this.context = context;
		}

		[HttpGet("get-custom-leaderboards")]
		[ProducesResponseType(200)]
		public ActionResult<List<CustomLeaderboardBase>> GetCustomLeaderboards()
			=> ApiFunctions.GetCustomLeaderboards(context);

		[HttpGet("get-deaths")]
		public ActionResult<List<Death>> GetDeaths(string gameVersion = GameInfo.DefaultGameVersion)
		{
			return ApiFunctions.GetDeaths(gameVersion);
		}

		public static Death GetDeath(int deathType, string gameVersion)
		{
			return GameInfo.GetDeathFromDeathType(deathType, gameVersion);
		}
	}
}