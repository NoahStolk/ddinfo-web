using DevilDaggersInfo.Api.Admin.Donations;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class DonationService
{
	private readonly ApplicationDbContext _dbContext;

	public DonationService(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task AddDonationAsync(AddDonation addDonation)
	{
		if (!_dbContext.Players.Any(p => p.Id == addDonation.PlayerId))
			throw new AdminDomainException($"Player with ID '{addDonation.PlayerId}' does not exist.");

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
	}

	public async Task EditDonationAsync(int id, EditDonation editDonation)
	{
		if (!_dbContext.Players.Any(p => p.Id == editDonation.PlayerId))
			throw new AdminDomainException($"Player with ID '{editDonation.PlayerId}' does not exist.");

		DonationEntity? donation = _dbContext.Donations.FirstOrDefault(d => d.Id == id);
		if (donation == null)
			throw new NotFoundException();

		donation.Amount = editDonation.Amount;
		donation.ConvertedEuroCentsReceived = editDonation.ConvertedEuroCentsReceived;
		donation.Currency = editDonation.Currency.ToDomain();
		donation.IsRefunded = editDonation.IsRefunded;
		donation.Note = editDonation.Note;
		donation.PlayerId = editDonation.PlayerId;
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteDonationAsync(int id)
	{
		DonationEntity? donation = _dbContext.Donations.FirstOrDefault(d => d.Id == id);
		if (donation == null)
			throw new NotFoundException();

		_dbContext.Donations.Remove(donation);
		await _dbContext.SaveChangesAsync();
	}
}
