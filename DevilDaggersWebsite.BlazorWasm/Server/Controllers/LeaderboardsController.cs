using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Clients.OfficialLeaderboard;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Leaderboards;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
{
	[Route("api/leaderboards")]
	[ApiController]
	public class LeaderboardsController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public async Task<ActionResult<GetLeaderboardPublic?>> GetLeaderboard([Range(1, int.MaxValue)] int rankStart = 1)
		{
			LeaderboardResponse l = await LeaderboardClient.Instance.GetScores(rankStart);
			return new GetLeaderboardPublic
			{
				DaggersFiredGlobal = l.DaggersFiredGlobal,
				DaggersHitGlobal = l.DaggersHitGlobal,
				DateTime = DateTime.UtcNow,
				DeathsGlobal = l.DeathsGlobal,
				Entries = l.Entries.ConvertAll(e => new GetEntryPublic
				{
					DaggersFired = e.DaggersFired,
					DaggersFiredTotal = e.DaggersFiredTotal,
					DaggersHit = e.DaggersHit,
					DaggersHitTotal = e.DaggersHitTotal,
					DeathsTotal = e.DeathsTotal,
					DeathType = (byte)e.DeathType,
					Gems = e.Gems,
					GemsTotal = e.GemsTotal,
					Id = e.Id,
					Kills = e.Kills,
					KillsTotal = e.KillsTotal,
					Rank = e.Rank,
					Time = e.Time == 0 ? 0 : e.Time / 10000f,
					TimeTotal = e.TimeTotal == 0 ? 0 : e.TimeTotal / 10000f,
					Username = e.Username,
				}),
				GemsGlobal = l.GemsGlobal,
				KillsGlobal = l.KillsGlobal,
				TotalPlayers = l.TotalPlayers,
				TimeGlobal = l.TimeGlobal == 0 ? 0 : l.TimeGlobal / 10000f,
			};
		}

		[HttpGet("user/by-id")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public async Task<ActionResult<GetEntryPublic>> GetPlayerById([Required, Range(1, int.MaxValue)] int userId)
		{
			EntryResponse e = await LeaderboardClient.Instance.GetUserById(userId);
			return new GetEntryPublic
			{
				DaggersFired = e.DaggersFired,
				DaggersFiredTotal = e.DaggersFiredTotal,
				DaggersHit = e.DaggersHit,
				DaggersHitTotal = e.DaggersHitTotal,
				DeathsTotal = e.DeathsTotal,
				DeathType = (byte)e.DeathType,
				Gems = e.Gems,
				GemsTotal = e.GemsTotal,
				Id = e.Id,
				Kills = e.Kills,
				KillsTotal = e.KillsTotal,
				Rank = e.Rank,
				Time = e.Time == 0 ? 0 : e.Time / 10000f,
				TimeTotal = e.TimeTotal == 0 ? 0 : e.TimeTotal / 10000f,
				Username = e.Username,
			};
		}

		[HttpGet("user/by-username")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public async Task<ActionResult<List<GetEntryPublic>>> GetPlayersByName([Required, MinLength(3)] string username)
		{
			List<EntryResponse> el = await LeaderboardClient.Instance.GetUserSearch(username);
			return el.ConvertAll(e => new GetEntryPublic
			{
				DaggersFired = e.DaggersFired,
				DaggersFiredTotal = e.DaggersFiredTotal,
				DaggersHit = e.DaggersHit,
				DaggersHitTotal = e.DaggersHitTotal,
				DeathsTotal = e.DeathsTotal,
				DeathType = (byte)e.DeathType,
				Gems = e.Gems,
				GemsTotal = e.GemsTotal,
				Id = e.Id,
				Kills = e.Kills,
				KillsTotal = e.KillsTotal,
				Rank = e.Rank,
				Time = e.Time == 0 ? 0 : e.Time / 10000f,
				TimeTotal = e.TimeTotal == 0 ? 0 : e.TimeTotal / 10000f,
				Username = e.Username,
			});
		}

		[HttpGet("user/by-rank")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public async Task<ActionResult<GetEntryPublic>> GetPlayerByRank([Required, Range(1, int.MaxValue)] int rank)
		{
			LeaderboardResponse l = await LeaderboardClient.Instance.GetScores(rank);
			if (l.Entries.Count == 0)
				return NotFound();

			EntryResponse e = l.Entries[0];
			return new GetEntryPublic
			{
				DaggersFired = e.DaggersFired,
				DaggersFiredTotal = e.DaggersFiredTotal,
				DaggersHit = e.DaggersHit,
				DaggersHitTotal = e.DaggersHitTotal,
				DeathsTotal = e.DeathsTotal,
				DeathType = (byte)e.DeathType,
				Gems = e.Gems,
				GemsTotal = e.GemsTotal,
				Id = e.Id,
				Kills = e.Kills,
				KillsTotal = e.KillsTotal,
				Rank = e.Rank,
				Time = e.Time == 0 ? 0 : e.Time / 10000f,
				TimeTotal = e.TimeTotal == 0 ? 0 : e.TimeTotal / 10000f,
				Username = e.Username,
			};
		}
	}
}
