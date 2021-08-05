using DevilDaggersWebsite.BlazorWasm.Server.Converters.Public;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums.Sortings.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Public
{
	[Route("api/custom-leaderboards")]
	[ApiController]
	public class CustomLeaderboardsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;

		public CustomLeaderboardsController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Page<GetCustomLeaderboardOverview>> GetCustomLeaderboards(
			CustomLeaderboardCategory categoryFilter,
			[Range(0, 1000)] int pageIndex = 0,
			[Range(PublicPagingConstants.PageSizeMin, PublicPagingConstants.PageSizeMax)] int pageSize = PublicPagingConstants.PageSizeDefault,
			CustomLeaderboardSorting? sortBy = null,
			bool ascending = false)
		{
			IQueryable<CustomLeaderboard> customLeaderboardsQuery = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.Where(cl => !cl.IsArchived && categoryFilter == cl.Category);

			customLeaderboardsQuery = sortBy switch
			{
				CustomLeaderboardSorting.AuthorName => customLeaderboardsQuery.OrderBy(cl => cl.SpawnsetFile.Player.PlayerName, ascending),
				CustomLeaderboardSorting.DateLastPlayed => customLeaderboardsQuery.OrderBy(cl => cl.DateLastPlayed, ascending),
				CustomLeaderboardSorting.SpawnsetName => customLeaderboardsQuery.OrderBy(cl => cl.SpawnsetFile.Name, ascending),
				CustomLeaderboardSorting.TimeBronze => customLeaderboardsQuery.OrderBy(cl => cl.TimeBronze, ascending),
				CustomLeaderboardSorting.TimeSilver => customLeaderboardsQuery.OrderBy(cl => cl.TimeSilver, ascending),
				CustomLeaderboardSorting.TimeGolden => customLeaderboardsQuery.OrderBy(cl => cl.TimeGolden, ascending),
				CustomLeaderboardSorting.TimeDevil => customLeaderboardsQuery.OrderBy(cl => cl.TimeDevil, ascending),
				CustomLeaderboardSorting.TimeLeviathan => customLeaderboardsQuery.OrderBy(cl => cl.TimeLeviathan, ascending),
				CustomLeaderboardSorting.DateCreated => customLeaderboardsQuery.OrderBy(cl => cl.DateCreated, ascending),
				_ => customLeaderboardsQuery,
			};

			List<CustomLeaderboard> customLeaderboards = customLeaderboardsQuery.ToList();

			IEnumerable<int> customLeaderboardIds = customLeaderboards.Select(cl => cl.Id);
			var customEntries = _dbContext.CustomEntries
				.AsNoTracking()
				.Where(ce => customLeaderboardIds.Contains(ce.CustomLeaderboardId))
				.Include(ce => ce.Player)
				.Select(ce => new { ce.Time, ce.Player.PlayerName, ce.CustomLeaderboardId });

			if (categoryFilter.IsAscending())
				customEntries = customEntries.OrderBy(wr => wr.Time);
			else
				customEntries = customEntries.OrderByDescending(wr => wr.Time);

			List<CustomLeaderboardWr> customLeaderboardWrs = customLeaderboards
				.ConvertAll(cl => new CustomLeaderboardWr(
					cl,
					customEntries.FirstOrDefault(wr => wr.CustomLeaderboardId == cl.Id)?.Time,
					customEntries.FirstOrDefault(wr => wr.CustomLeaderboardId == cl.Id)?.PlayerName));

			if (sortBy is CustomLeaderboardSorting.WorldRecord)
			{
				customLeaderboardWrs = ascending
					? customLeaderboardWrs.OrderBy(wr => wr.WorldRecord).ToList()
					: customLeaderboardWrs.OrderByDescending(wr => wr.WorldRecord).ToList();
			}
			else if (sortBy is CustomLeaderboardSorting.TopPlayer)
			{
				customLeaderboardWrs = ascending
					? customLeaderboardWrs.OrderBy(wr => wr.TopPlayer).ToList()
					: customLeaderboardWrs.OrderByDescending(wr => wr.TopPlayer).ToList();
			}

			customLeaderboardWrs = customLeaderboardWrs
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetCustomLeaderboardOverview>
			{
				Results = customLeaderboardWrs.ConvertAll(cl => cl.CustomLeaderboard.ToGetCustomLeaderboardOverview(cl.TopPlayer, cl.WorldRecord)),
				TotalResults = customLeaderboardsQuery.Count(),
			};
		}

		private class CustomLeaderboardWr
		{
			public CustomLeaderboardWr(CustomLeaderboard customLeaderboard, int? worldRecord, string? topPlayer)
			{
				CustomLeaderboard = customLeaderboard;
				WorldRecord = worldRecord;
				TopPlayer = topPlayer;
			}

			public CustomLeaderboard CustomLeaderboard { get; }
			public int? WorldRecord { get; }
			public string? TopPlayer { get; }
		}
	}
}
