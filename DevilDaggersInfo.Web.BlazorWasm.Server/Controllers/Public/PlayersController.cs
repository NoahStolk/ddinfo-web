using DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public
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

		[HttpGet("leaderboard")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public ActionResult<List<GetPlayerForLeaderboard>> GetPlayersForLeaderboard()
		{
			var players = _dbContext.Players
				.AsNoTracking()
				.Select(p => new { p.Id, p.BanDescription, p.IsBanned, p.CountryCode, p.HideDonations })
				.ToList();

			var donations = _dbContext.Donations
				.AsNoTracking()
				.Select(d => new { d.PlayerId, d.IsRefunded, d.ConvertedEuroCentsReceived })
				.Where(d => !d.IsRefunded && d.ConvertedEuroCentsReceived > 0)
				.ToList();

			List<PlayerTitleEntity> playerTitles = _dbContext.PlayerTitles
				.AsNoTracking()
				.Include(pt => pt.Title)
				.ToList();

			return players.ConvertAll(p => new GetPlayerForLeaderboard
			{
				Id = p.Id,
				BanDescription = p.BanDescription,
				IsBanned = p.IsBanned,
				IsPublicDonator = donations.Any(d => d.PlayerId == p.Id),
				Titles = playerTitles.Where(pt => pt.PlayerId == p.Id).Select(pt => pt.Title.Name).ToList(),
				CountryCode = p.CountryCode,
			});
		}
	}
}
