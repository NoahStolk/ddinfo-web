using DevilDaggersInfo.Web.ApiSpec.Admin.Donations;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public sealed class DonationService(ApplicationDbContext dbContext)
{
	public async Task AddDonationAsync(AddDonation addDonation)
	{
		if (!dbContext.Players.Any(p => p.Id == addDonation.PlayerId))
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
		dbContext.Donations.Add(donation);
		await dbContext.SaveChangesAsync();
	}

	public async Task EditDonationAsync(int id, EditDonation editDonation)
	{
		if (!dbContext.Players.Any(p => p.Id == editDonation.PlayerId))
			throw new AdminDomainException($"Player with ID '{editDonation.PlayerId}' does not exist.");

		DonationEntity? donation = dbContext.Donations.FirstOrDefault(d => d.Id == id);
		if (donation == null)
			throw new NotFoundException();

		donation.Amount = editDonation.Amount;
		donation.ConvertedEuroCentsReceived = editDonation.ConvertedEuroCentsReceived;
		donation.Currency = editDonation.Currency.ToDomain();
		donation.IsRefunded = editDonation.IsRefunded;
		donation.Note = editDonation.Note;
		donation.PlayerId = editDonation.PlayerId;
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteDonationAsync(int id)
	{
		DonationEntity? donation = dbContext.Donations.FirstOrDefault(d => d.Id == id);
		if (donation == null)
			throw new NotFoundException();

		dbContext.Donations.Remove(donation);
		await dbContext.SaveChangesAsync();
	}
}
