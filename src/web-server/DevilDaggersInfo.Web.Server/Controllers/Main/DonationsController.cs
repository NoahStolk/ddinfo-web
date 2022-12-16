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

	[HttpGet("donators")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<List<GetDonator>> GetDonators()
		=> await _donationRepository.GetDonatorsAsync();
}
