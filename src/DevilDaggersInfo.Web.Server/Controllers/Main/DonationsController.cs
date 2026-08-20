using DevilDaggersInfo.Web.ApiSpec.Main.Donations;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/donations")]
[ApiController]
public sealed class DonationsController(DonationRepository donationRepository) : ControllerBase
{
	[HttpGet("donors")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<List<GetDonor>> GetDonors()
	{
		return await donationRepository.GetDonorsAsync();
	}
}
