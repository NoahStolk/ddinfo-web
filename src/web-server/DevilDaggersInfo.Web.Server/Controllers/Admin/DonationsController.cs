using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Donations;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/donations")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class DonationsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly DonationRepository _donationRepository;

	public DonationsController(ApplicationDbContext dbContext, DonationRepository donationRepository)
	{
		_dbContext = dbContext;
		_donationRepository = donationRepository;
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
		if (!_dbContext.Players.Any(p => p.Id == addDonation.PlayerId))
			return BadRequest($"Player with ID '{addDonation.PlayerId}' does not exist.");

		DonationEntity donation = new()
		{
			Amount = addDonation.Amount,
			ConvertedEuroCentsReceived = addDonation.ConvertedEuroCentsReceived,
			Currency = addDonation.Currency,
			DateReceived = DateTime.UtcNow,
			IsRefunded = addDonation.IsRefunded,
			Note = addDonation.Note,
			PlayerId = addDonation.PlayerId,
		};
		_dbContext.Donations.Add(donation);
		await _dbContext.SaveChangesAsync();

		return Ok(donation.Id);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditDonationById(int id, EditDonation editDonation)
	{
		if (!_dbContext.Players.Any(p => p.Id == editDonation.PlayerId))
			return BadRequest($"Player with ID '{editDonation.PlayerId}' does not exist.");

		DonationEntity? donation = _dbContext.Donations.FirstOrDefault(d => d.Id == id);
		if (donation == null)
			return NotFound();

		donation.Amount = editDonation.Amount;
		donation.ConvertedEuroCentsReceived = editDonation.ConvertedEuroCentsReceived;
		donation.Currency = editDonation.Currency;
		donation.IsRefunded = editDonation.IsRefunded;
		donation.Note = editDonation.Note;
		donation.PlayerId = editDonation.PlayerId;
		await _dbContext.SaveChangesAsync();

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteDonationById(int id)
	{
		DonationEntity? donation = _dbContext.Donations.FirstOrDefault(d => d.Id == id);
		if (donation == null)
			return NotFound();

		_dbContext.Donations.Remove(donation);
		await _dbContext.SaveChangesAsync();

		return Ok();
	}
}
