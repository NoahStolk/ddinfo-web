using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Donations;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public class DonationRepository
{
	private readonly ApplicationDbContext _dbContext;

	public DonationRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Page<GetDonationForOverview>> GetDonationsAsync(string? filter, int pageIndex, int pageSize, DonationSorting? sortBy, bool ascending)
	{
		IQueryable<DonationEntity> donationsQuery = _dbContext.Donations
			.AsNoTracking()
			.Include(d => d.Player);

		if (!string.IsNullOrWhiteSpace(filter))
		{
			// ! Navigation property.
			donationsQuery = donationsQuery.Where(d => d.Player!.PlayerName.Contains(filter));
		}

		// ! Navigation property.
		donationsQuery = sortBy switch
		{
			DonationSorting.Amount => donationsQuery.OrderBy(d => d.Amount, ascending).ThenBy(d => d.Id),
			DonationSorting.ConvertedEuroCentsReceived => donationsQuery.OrderBy(d => d.ConvertedEuroCentsReceived, ascending).ThenBy(d => d.Id),
			DonationSorting.Currency => donationsQuery.OrderBy(d => d.Currency, ascending).ThenBy(d => d.Id),
			DonationSorting.DateReceived => donationsQuery.OrderBy(d => d.DateReceived, ascending).ThenBy(d => d.Id),
			DonationSorting.IsRefunded => donationsQuery.OrderBy(d => d.IsRefunded, ascending).ThenBy(d => d.Id),
			DonationSorting.Note => donationsQuery.OrderBy(d => d.Note, ascending).ThenBy(d => d.Id),
			DonationSorting.PlayerName => donationsQuery.OrderBy(d => d.Player!.PlayerName, ascending).ThenBy(d => d.Id),
			_ => donationsQuery.OrderBy(d => d.Id, ascending),
		};

		List<DonationEntity> donations = await donationsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return new Page<GetDonationForOverview>
		{
			Results = donations.ConvertAll(d => d.ToAdminApiOverview()),
			TotalResults = await donationsQuery.CountAsync(),
		};
	}

	public async Task<GetDonation> GetDonationAsync(int id)
	{
		DonationEntity? donation = await _dbContext.Donations
			.AsNoTracking()
			.FirstOrDefaultAsync(d => d.Id == id);
		if (donation == null)
			throw new NotFoundException();

		return donation.ToAdminApi();
	}
}
