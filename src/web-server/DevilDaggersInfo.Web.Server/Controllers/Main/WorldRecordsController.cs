using DevilDaggersInfo.Api.Main.WorldRecords;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/world-records")]
[ApiController]
public class WorldRecordsController : ControllerBase
{
	private readonly WorldRecordRepository _worldRecordRepository;

	public WorldRecordsController(WorldRecordRepository worldRecordRepository)
	{
		_worldRecordRepository = worldRecordRepository;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public GetWorldRecordDataContainer GetWorldRecordData()
		=> _worldRecordRepository.GetWorldRecordData();
}
