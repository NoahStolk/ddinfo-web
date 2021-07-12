using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Players;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
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

			List<PlayerTitle> playerTitles = _dbContext.PlayerTitles
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
