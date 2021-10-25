using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevilDaggersWebsite.Api
{
	[Route("api/players")]
	[ApiController]
	public class PlayersController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;

		public PlayersController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet("{id}/flag")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<string> GetPlayerFlagById([Required] int id)
		{
			var player = _dbContext.Players.AsNoTracking().Select(p => new { p.Id, p.CountryCode }).FirstOrDefault(p => p.Id == id);
			return player?.CountryCode ?? string.Empty;
		}
	}
}
