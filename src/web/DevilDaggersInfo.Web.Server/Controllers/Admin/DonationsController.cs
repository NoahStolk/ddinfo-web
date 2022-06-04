using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Donations;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/donations")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
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
	public ActionResult<Page<GetDonationForOverview>> GetDonations(
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

		return new Page<GetDonationForOverview>
		{
			Results = donations.ConvertAll(d => d.ToGetDonationForOverview()),
			TotalResults = _dbContext.Donations.Count(),
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public ActionResult<GetDonation> GetDonationById(int id)
	{
		DonationEntity? donation = _dbContext.Donations
			.AsNoTracking()
			.FirstOrDefault(d => d.Id == id);
		if (donation == null)
			return NotFound();

		return donation.ToGetDonation();
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
			Currency = addDonation.Currency.ToDomain(),
			DateReceived = DateTime.UtcNow,
			IsRefunded = addDonation.IsRefunded,
			Note = addDonation.Note,
			PlayerId = addDonation.PlayerId,
		};
		_dbContext.Donations.Add(donation);
		await _dbContext.SaveChangesAsync();

		_auditLogger.LogAdd(addDonation.GetLog(), User, donation.Id);

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
			Currency = donation.Currency.ToAdminApi(),
			IsRefunded = donation.IsRefunded,
			Note = donation.Note,
			PlayerId = donation.PlayerId,
		};

		donation.Amount = editDonation.Amount;
		donation.ConvertedEuroCentsReceived = editDonation.ConvertedEuroCentsReceived;
		donation.Currency = editDonation.Currency.ToDomain();
		donation.IsRefunded = editDonation.IsRefunded;
		donation.Note = editDonation.Note;
		donation.PlayerId = editDonation.PlayerId;
		await _dbContext.SaveChangesAsync();

		_auditLogger.LogEdit(logDto.GetLog(), editDonation.GetLog(), User, donation.Id);

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

		_auditLogger.LogDelete(donation.GetLog(), User, donation.Id);

		return Ok();
	}
}
