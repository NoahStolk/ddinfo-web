using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Api.Main.Spawnsets;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Main.Services;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly PlayerRepository _playerRepository;
	private readonly PlayerHistoryRepository _playerHistoryRepository;
	private readonly PlayerProfileService _profileService;
	private readonly PlayerProfileRepository _profileRepository;

	public PlayersController(ApplicationDbContext dbContext, PlayerRepository playerRepository, PlayerHistoryRepository playerHistoryRepository, PlayerProfileService profileService, PlayerProfileRepository profileRepository)
	{
		_dbContext = dbContext;
		_playerRepository = playerRepository;
		_playerHistoryRepository = playerHistoryRepository;
		_profileService = profileService;
		_profileRepository = profileRepository;
	}

	[HttpGet("leaderboard")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerForLeaderboard>>> GetPlayersForLeaderboard()
	{
		List<Domain.Models.Players.PlayerForLeaderboard> players = await _playerRepository.GetPlayersForLeaderboardAsync();
		return players.ConvertAll(p => p.ToMainApi());
	}

	[HttpGet("settings")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerForSettings>>> GetPlayersForSettings()
	{
		List<Domain.Models.Players.PlayerForSettings> players = await _playerRepository.GetPlayersForSettingsAsync();
		return players.ConvertAll(p => p.ToMainApi());
	}

	// FORBIDDEN: Used by DDLIVE.
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetPlayer>> GetPlayerById([Required] int id)
	{
		Domain.Models.Players.Player player = await _playerRepository.GetPlayerAsync(id);
		return player.ToMainApi();
	}

	// FORBIDDEN: Used by DDLIVE.
	[HttpGet("{id}/history")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public GetPlayerHistory GetPlayerHistoryById([Required, Range(1, int.MaxValue)] int id)
	{
		return _playerHistoryRepository.GetPlayerHistoryById(id);
	}

	[HttpGet("{id}/custom-leaderboard-statistics")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerCustomLeaderboardStatistics>>> GetCustomLeaderboardStatisticsByPlayerId([Required] int id)
	{
		// ! Navigation property.
		var customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.CustomLeaderboard)
			.Where(cl => cl.PlayerId == id)
			.Select(ce => new
			{
				ce.Time,
				ce.CustomLeaderboardId,
				ce.CustomLeaderboard!.Spawnset!.GameMode,
				ce.CustomLeaderboard!.RankSorting,
				ce.CustomLeaderboard.Leviathan,
				ce.CustomLeaderboard.Devil,
				ce.CustomLeaderboard.Golden,
				ce.CustomLeaderboard.Silver,
				ce.CustomLeaderboard.Bronze,
				ce.CustomLeaderboard.IsFeatured,
			})
			.Where(ce => ce.IsFeatured)
			.ToListAsync();

		List<GetPlayerCustomLeaderboardStatistics> stats = new();
		foreach (SpawnsetGameMode gameMode in Enum.GetValues<SpawnsetGameMode>())
		{
			var customEntriesByCategory = customEntries.Where(ce => ce.GameMode == gameMode).ToList();
			if (customEntriesByCategory.Count == 0)
				continue;

			foreach (CustomLeaderboardRankSorting rankSorting in Enum.GetValues<CustomLeaderboardRankSorting>())
			{
				var customEntriesByRankSorting = customEntriesByCategory.Where(ce => ce.RankSorting == rankSorting).ToList();

				int leviathanDaggers = 0;
				int devilDaggers = 0;
				int goldenDaggers = 0;
				int silverDaggers = 0;
				int bronzeDaggers = 0;
				int defaultDaggers = 0;
				int played = 0;
				foreach (var customEntry in customEntriesByCategory)
				{
					played++;
					switch (CustomLeaderboardUtils.GetDaggerFromStat(rankSorting, customEntry.Time, customEntry.Leviathan, customEntry.Devil, customEntry.Golden, customEntry.Silver, customEntry.Bronze))
					{
						case CustomLeaderboardDagger.Leviathan: leviathanDaggers++; break;
						case CustomLeaderboardDagger.Devil: devilDaggers++; break;
						case CustomLeaderboardDagger.Golden: goldenDaggers++; break;
						case CustomLeaderboardDagger.Silver: silverDaggers++; break;
						case CustomLeaderboardDagger.Bronze: bronzeDaggers++; break;
						default: defaultDaggers++; break;
					}
				}

				stats.Add(new()
				{
					GameMode = gameMode.ToMainApi(),
					RankSorting = rankSorting.ToMainApi(),
					LeviathanDaggerCount = leviathanDaggers,
					DevilDaggerCount = devilDaggers,
					GoldenDaggerCount = goldenDaggers,
					SilverDaggerCount = silverDaggers,
					BronzeDaggerCount = bronzeDaggers,
					DefaultDaggerCount = defaultDaggers,
					LeaderboardsPlayedCount = played,
					TotalCount = customEntriesByRankSorting.Count,
				});
			}
		}

		return stats;
	}

	[Authorize]
	[HttpGet("{id}/profile")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetPlayerProfile>> GetProfileByPlayerId([Required] int id)
		=> await _profileRepository.GetProfileAsync(User, id);

	[Authorize]
	[HttpPut("{id}/profile")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> UpdateProfileByPlayerId([Required] int id, EditPlayerProfile editPlayerProfile)
	{
		await _profileService.UpdateProfileAsync(User, id, editPlayerProfile);
		return Ok();
	}
}
