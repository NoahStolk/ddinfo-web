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

	public async Task<List<GetDonor>> GetDonorsAsync()
	{
		var donations = await _dbContext.Donations
			.AsNoTracking()
			.Include(d => d.Player)
			.Select(d => new { d.Amount, d.ConvertedEuroCentsReceived, d.Currency, d.IsRefunded, d.PlayerId })
			.Where(d => !d.IsRefunded && d.ConvertedEuroCentsReceived > 0)
			.ToListAsync();

		List<int> donorIds = donations.ConvertAll(d => d.PlayerId);
		var donors = await _dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.HideDonations, p.PlayerName })
			.Where(p => donorIds.Contains(p.Id))
			.ToListAsync();

		return donors
			.ConvertAll(
				p => new GetDonor
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
			.OrderByDescending(donor => donor.Donations.Sum(donation => donation.ConvertedEuroCentsReceived))
			.ToList();
	}
}
