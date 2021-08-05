using DevilDaggersWebsite.BlazorWasm.Server.Converters.Public;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
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
			[Range(10, 25)] int pageSize = 25,
			CustomLeaderboardSorting? sortBy = null,
			bool ascending = false,
			CustomLeaderboardCategory? categoriesFilter = null)
		{
			IQueryable<CustomLeaderboard> customLeaderboardsQuery = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
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
				_ => customLeaderboardsQuery.OrderBy(cl => cl.DateCreated, ascending),
			};

			if (categoriesFilter != null)
				customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => categoriesFilter == cl.Category);

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
