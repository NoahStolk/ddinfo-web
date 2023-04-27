using DevilDaggersInfo.Api.Main.Donations;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Main.Converters;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Repositories;

public class DonationRepository
{
	private readonly ApplicationDbContext _dbContext;

	public DonationRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<GetDonator>> GetDonatorsAsync()
	{
		var donations = await _dbContext.Donations
			.AsNoTracking()
			.Include(d => d.Player)
			.Select(d => new { d.Amount, d.ConvertedEuroCentsReceived, d.Currency, d.IsRefunded, d.PlayerId })
			.Where(d => !d.IsRefunded && d.ConvertedEuroCentsReceived > 0)
			.ToListAsync();

		List<int> donatorIds = donations.ConvertAll(d => d.PlayerId);
		var donators = await _dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.HideDonations, p.PlayerName })
			.Where(p => donatorIds.Contains(p.Id))
			.ToListAsync();

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
