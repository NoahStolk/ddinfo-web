using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.CustomEntries;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/custom-entries")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class CustomEntriesController : ControllerBase
{
	private readonly CustomEntryRepository _customEntryRepository;
	private readonly CustomEntryService _customEntryService;

	public CustomEntriesController(CustomEntryRepository customEntryRepository, CustomEntryService customEntryService)
	{
		_customEntryRepository = customEntryRepository;
		_customEntryService = customEntryService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<Page<GetCustomEntryForOverview>>> GetCustomEntries(
		string? filter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		CustomEntrySorting? sortBy = null,
		bool ascending = false)
		=> await _customEntryRepository.GetCustomEntriesAsync(filter, pageIndex, pageSize, sortBy, ascending);

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomEntry>> GetCustomEntryById(int id)
		=> await _customEntryRepository.GetCustomEntryAsync(id);

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddCustomEntry(AddCustomEntry addCustomEntry)
	{
		await _customEntryService.AddCustomEntryAsync(addCustomEntry);
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditCustomEntryById(int id, EditCustomEntry editCustomEntry)
	{
		await _customEntryService.EditCustomEntryAsync(id, editCustomEntry);
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteCustomEntryById(int id)
	{
		await _customEntryService.DeleteCustomEntryAsync(id);
		return Ok();
	}
}
