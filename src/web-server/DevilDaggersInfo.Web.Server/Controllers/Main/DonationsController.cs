using DevilDaggersInfo.Api.Main.Donations;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/donations")]
[ApiController]
public class DonationsController : ControllerBase
{
	private readonly DonationRepository _donationRepository;

	public DonationsController(DonationRepository donationRepository)
	{
		_donationRepository = donationRepository;
	}

	// TODO: Rename to donors.
	[HttpGet("donators")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<List<GetDonor>> GetDonors()
		=> await _donationRepository.GetDonorsAsync();
}
