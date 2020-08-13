using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersCore.Website;
using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api")]
	[ApiController]
	public class LegacyController : ControllerBase
	{
		private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment env;

		public LegacyController(ApplicationDbContext context, IWebHostEnvironment env)
		{
			this.context = context;
			this.env = env;
		}

		[Obsolete("api/custom-leaderboards")]
		[HttpGet("GetCustomLeaderboards")]
		[ProducesResponseType(200)]
		public ActionResult<List<CustomLeaderboardBase>> GetCustomLeaderboards()
			=> new CustomLeaderboardsController(context).GetCustomLeaderboards();

		[Obsolete("api/leaderboards")]
		[HttpGet("GetLeaderboard")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<Leaderboard>> GetLeaderboard(int rank = 1)
			=> await new LeaderboardsController().GetLeaderboard(rank);

		[Obsolete("api/spawnsets/{fileName}/path")]
		[HttpGet("GetSpawnset")]
		[ProducesResponseType(200)]
		public ActionResult GetSpawnset(string fileName)
			=> new SpawnsetsController(env).GetSpawnset(fileName);

		[Obsolete("api/spawnsets")]
		[HttpGet("GetSpawnsets")]
		[ProducesResponseType(200)]
		public ActionResult<List<SpawnsetFile>> GetSpawnsets(string searchAuthor = null, string searchName = null)
			=> new SpawnsetsController(env).GetSpawnsets(searchAuthor, searchName);

		[Obsolete("api/tools/{toolName}/path")]
		[HttpGet("GetTool")]
		[ProducesResponseType(200)]
		public ActionResult GetTool(string toolName)
			=> new ToolsController(env).GetTool(toolName);

		[Obsolete("api/tools")]
		[HttpGet("GetTools")]
		[ProducesResponseType(200)]
		public ActionResult<List<Tool>> GetTools()
			=> new ToolsController(env).GetTools();

		[Obsolete("api/leaderboards/user/by-id")]
		[HttpGet("GetUserById")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<Entry>> GetUserById(int userId)
			=> await new LeaderboardsController().GetUserById(userId);

		[Obsolete("api/leaderboards/user/by-rank")]
		[HttpGet("GetUserByRank")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<Entry>> GetUserByRank(int rank)
			=> await new LeaderboardsController().GetUserByRank(rank);

		[Obsolete("api/leaderboards/user/by-username")]
		[HttpGet("GetUserByUsername")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<List<Entry>>> GetUserByUsername(string username)
			=> await new LeaderboardsController().GetUserByUsername(username);
	}
}