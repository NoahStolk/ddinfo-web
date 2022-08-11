using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/custom-leaderboards")]
[ApiController]
[Authorize(Roles = Roles.CustomLeaderboards)]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly CustomLeaderboardRepository _customLeaderboardRepository;
	private readonly CustomLeaderboardService _customLeaderboardService;

	public CustomLeaderboardsController(CustomLeaderboardRepository customLeaderboardRepository, CustomLeaderboardService customLeaderboardService)
	{
		_customLeaderboardRepository = customLeaderboardRepository;
		_customLeaderboardService = customLeaderboardService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<Page<GetCustomLeaderboardForOverview>>> GetCustomLeaderboards(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		CustomLeaderboardSorting? sortBy = null,
		bool ascending = false)
		=> await _customLeaderboardRepository.GetCustomLeaderboardsAsync(pageIndex, pageSize, sortBy, ascending);

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardById(int id)
		=> await _customLeaderboardRepository.GetCustomLeaderboardAsync(id);

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddCustomLeaderboard(AddCustomLeaderboard addCustomLeaderboard)
	{
		await _customLeaderboardService.AddCustomLeaderboardAsync(new Domain.Admin.Commands.CustomLeaderboards.AddCustomLeaderboard
		{
			Category = addCustomLeaderboard.Category.ToDomain(),
			Daggers = new Domain.Admin.Commands.CustomLeaderboards.Models.CustomLeaderboardDaggers
			{
				Bronze = addCustomLeaderboard.Daggers.Bronze.To10thMilliTime(),
				Silver = addCustomLeaderboard.Daggers.Silver.To10thMilliTime(),
				Golden = addCustomLeaderboard.Daggers.Golden.To10thMilliTime(),
				Devil = addCustomLeaderboard.Daggers.Devil.To10thMilliTime(),
				Leviathan = addCustomLeaderboard.Daggers.Leviathan.To10thMilliTime(),
			},
			IsFeatured = addCustomLeaderboard.IsFeatured,
			SpawnsetId = addCustomLeaderboard.SpawnsetId,
		});
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditCustomLeaderboardById(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		await _customLeaderboardService.EditCustomLeaderboardAsync(new Domain.Admin.Commands.CustomLeaderboards.EditCustomLeaderboard
		{
			Category = editCustomLeaderboard.Category.ToDomain(),
			Daggers = new Domain.Admin.Commands.CustomLeaderboards.Models.CustomLeaderboardDaggers
			{
				Bronze = editCustomLeaderboard.Daggers.Bronze.To10thMilliTime(),
				Silver = editCustomLeaderboard.Daggers.Silver.To10thMilliTime(),
				Golden = editCustomLeaderboard.Daggers.Golden.To10thMilliTime(),
				Devil = editCustomLeaderboard.Daggers.Devil.To10thMilliTime(),
				Leviathan = editCustomLeaderboard.Daggers.Leviathan.To10thMilliTime(),
			},
			Id = id,
			IsFeatured = editCustomLeaderboard.IsFeatured,
		});
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteCustomLeaderboardById(int id)
	{
		await _customLeaderboardService.DeleteCustomLeaderboardAsync(id);
		return Ok();
	}
}
