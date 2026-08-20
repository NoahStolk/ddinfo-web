using DevilDaggersInfo.Web.ApiSpec.Main.WorldRecords;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/world-records")]
[ApiController]
public sealed class WorldRecordsController(WorldRecordRepository worldRecordRepository) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<GetWorldRecordDataContainer> GetWorldRecordData()
	{
		return await worldRecordRepository.GetWorldRecordDataAsync();
	}
}
