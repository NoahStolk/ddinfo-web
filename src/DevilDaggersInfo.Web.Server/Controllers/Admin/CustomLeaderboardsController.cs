using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
		string? filter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		CustomLeaderboardSorting? sortBy = null,
		bool ascending = false)
	{
		return await _customLeaderboardRepository.GetCustomLeaderboardsAsync(filter, pageIndex, pageSize, sortBy, ascending);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardById(int id)
	{
		return await _customLeaderboardRepository.GetCustomLeaderboardAsync(id);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddCustomLeaderboard(AddCustomLeaderboard addCustomLeaderboard)
	{
		await _customLeaderboardService.AddCustomLeaderboardAsync(addCustomLeaderboard);
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditCustomLeaderboardById(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		await _customLeaderboardService.EditCustomLeaderboardAsync(id, editCustomLeaderboard);
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
