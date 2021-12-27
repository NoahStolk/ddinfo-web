using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Donations;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Admin;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/donations")]
[ApiController]
[Authorize(Roles = Roles.Donations)]
public class DonationsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly AuditLogger _auditLogger;

	public DonationsController(ApplicationDbContext dbContext, AuditLogger auditLogger)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Page<GetDonation>> GetDonations(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(PagingConstants.PageSizeMin, PagingConstants.PageSizeMax)] int pageSize = PagingConstants.PageSizeDefault,
		DonationSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<DonationEntity> donationsQuery = _dbContext.Donations
			.AsNoTracking()
			.Include(d => d.Player);

		donationsQuery = sortBy switch
		{
			DonationSorting.Amount => donationsQuery.OrderBy(d => d.Amount, ascending).ThenBy(d => d.Id),
			DonationSorting.ConvertedEuroCentsReceived => donationsQuery.OrderBy(d => d.ConvertedEuroCentsReceived, ascending).ThenBy(d => d.Id),
			DonationSorting.Currency => donationsQuery.OrderBy(d => d.Currency, ascending).ThenBy(d => d.Id),
			DonationSorting.DateReceived => donationsQuery.OrderBy(d => d.DateReceived, ascending).ThenBy(d => d.Id),
			DonationSorting.IsRefunded => donationsQuery.OrderBy(d => d.IsRefunded, ascending).ThenBy(d => d.Id),
			DonationSorting.Note => donationsQuery.OrderBy(d => d.Note, ascending).ThenBy(d => d.Id),
			DonationSorting.PlayerName => donationsQuery.OrderBy(d => d.Player.PlayerName, ascending).ThenBy(d => d.Id),
			_ => donationsQuery.OrderBy(d => d.Id, ascending),
		};

		List<DonationEntity> donations = donationsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetDonation>
		{
			Results = donations.ConvertAll(d => d.ToGetDonation()),
			TotalResults = _dbContext.Donations.Count(),
		};
	}

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
			DateReceived = addDonation.DateReceived,
			IsRefunded = addDonation.IsRefunded,
			Note = addDonation.Note,
			PlayerId = addDonation.PlayerId,
		};
		_dbContext.Donations.Add(donation);
		_dbContext.SaveChanges();

		await _auditLogger.LogAdd(addDonation, User, donation.Id);

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

		EditDonation logDto = new()
		{
			Amount = donation.Amount,
			ConvertedEuroCentsReceived = donation.ConvertedEuroCentsReceived,
			Currency = donation.Currency,
			DateReceived = donation.DateReceived,
			IsRefunded = donation.IsRefunded,
			Note = donation.Note,
			PlayerId = donation.PlayerId,
		};

		donation.Amount = editDonation.Amount;
		donation.ConvertedEuroCentsReceived = editDonation.ConvertedEuroCentsReceived;
		donation.Currency = editDonation.Currency;
		donation.DateReceived = editDonation.DateReceived;
		donation.IsRefunded = editDonation.IsRefunded;
		donation.Note = editDonation.Note;
		donation.PlayerId = editDonation.PlayerId;
		_dbContext.SaveChanges();

		await _auditLogger.LogEdit(logDto, editDonation, User, donation.Id);

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
		_dbContext.SaveChanges();

		await _auditLogger.LogDelete(donation, User, donation.Id);

		return Ok();
	}
}
