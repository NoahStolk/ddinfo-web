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
			[Range(0, 1000)] int pageIndex = 0,
			[Range(PublicPagingConstants.PageSizeMin, PublicPagingConstants.PageSizeMax)] int pageSize = PublicPagingConstants.PageSizeDefault,
			CustomLeaderboardSorting? sortBy = null,
			bool ascending = false,
			CustomLeaderboardCategory? categoryFilter = null)
		{
			IQueryable<CustomLeaderboard> customLeaderboardsQuery = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.Include(cl => cl.CustomEntries)
					.ThenInclude(ce => ce.Player)
				.Where(cl => !cl.IsArchived);

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

				// TODO: Take leaderboard direction into account.
				CustomLeaderboardSorting.TopPlayer => customLeaderboardsQuery.OrderBy(cl => cl.CustomEntries != null && cl.CustomEntries.Count > 0 ? cl.CustomEntries.OrderBy(ce => ce.Time).First().Player.PlayerName : string.Empty),
				CustomLeaderboardSorting.WorldRecord => customLeaderboardsQuery.OrderBy(cl => cl.CustomEntries != null && cl.CustomEntries.Count > 0 ? cl.CustomEntries.Max(ce => ce.Time) : 0),
				_ => customLeaderboardsQuery.OrderBy(cl => cl.DateCreated, ascending),
			};

			if (categoryFilter != null)
				customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => categoryFilter == cl.Category);

			List<CustomLeaderboard> customLeaderboards = customLeaderboardsQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetCustomLeaderboardOverview>
			{
				Results = customLeaderboards.ConvertAll(cl => cl.ToGetCustomLeaderboardOverview()),
				TotalResults = customLeaderboardsQuery.Count(),
			};
		}
	}
}
