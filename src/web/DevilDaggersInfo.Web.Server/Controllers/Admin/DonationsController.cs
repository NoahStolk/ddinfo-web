using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Donations;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/donations")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class DonationsController : ControllerBase
{
	private readonly DonationRepository _donationRepository;
	private readonly DonationService _donationService;

	public DonationsController(DonationRepository donationRepository, DonationService donationService)
	{
		_donationRepository = donationRepository;
		_donationService = donationService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<Page<GetDonationForOverview>>> GetDonations(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		DonationSorting? sortBy = null,
		bool ascending = false)
		=> await _donationRepository.GetDonationsAsync(pageIndex, pageSize, sortBy, ascending);

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult<GetDonation>> GetDonationById(int id)
		=> await _donationRepository.GetDonationAsync(id);

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddDonation(AddDonation addDonation)
	{
		await _donationService.AddDonationAsync(addDonation);
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditDonationById(int id, EditDonation editDonation)
	{
		await _donationService.EditDonationAsync(id, editDonation);
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteDonationById(int id)
	{
		await _donationService.DeleteDonationAsync(id);
		return Ok();
	}
}
