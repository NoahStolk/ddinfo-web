using DevilDaggersInfo.Api.Main.Donations;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/donations")]
[ApiController]
public class DonationsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public DonationsController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet("donators")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetDonator> GetDonators()
	{
		var donations = _dbContext.Donations
			.AsNoTracking()
			.Include(d => d.Player)
			.Select(d => new { d.Amount, d.ConvertedEuroCentsReceived, d.Currency, d.IsRefunded, d.PlayerId })
			.Where(d => !d.IsRefunded && d.ConvertedEuroCentsReceived > 0)
			.ToList();

		List<int> donatorIds = donations.ConvertAll(d => d.PlayerId);
		var donators = _dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.HideDonations, p.PlayerName })
			.Where(p => donatorIds.Contains(p.Id))
			.ToList();

		return donators
			.ConvertAll(
				p => new GetDonator
				{
					Donations = donations
						.Where(d => d.PlayerId == p.Id)
						.Select(d => new GetDonation
						{
							Amount = d.Amount,
							ConvertedEuroCentsReceived = d.ConvertedEuroCentsReceived,
							Currency = d.Currency.ToMainApi(),
							IsRefunded = d.IsRefunded,
						})
						.ToList(),
					PlayerId = p.HideDonations ? null : p.Id,
					PlayerName = p.HideDonations ? "(anonymous)" : p.PlayerName,
				})
			.OrderByDescending(d => d.Donations.Sum(d => d.ConvertedEuroCentsReceived))
			.ToList();
	}
}
