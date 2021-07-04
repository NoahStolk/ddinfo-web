using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Dto.CustomLeaderboards;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Api
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
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<List<GetCustomLeaderboard>> GetCustomLeaderboards()
		{
			return _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.Select(cl => cl.ToDto())
				.ToList();
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<GetCustomLeaderboard> GetCustomLeaderboard(int id)
		{
			CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.FirstOrDefault(cl => cl.Id == id);

			if (customLeaderboard == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Leaderboard with {nameof(id)} '{id}' was not found." });

			return customLeaderboard.ToDto();
		}
	}
}
