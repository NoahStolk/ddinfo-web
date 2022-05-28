using DevilDaggersInfo.Web.Server.Repositories;

namespace DevilDaggersInfo.Web.Server.Controllers.DdLive;

[Route("api/ddlive/custom-entries")]
[ApiController]
public class CustomEntriesController : ControllerBase
{
	private readonly CustomEntryRepository _customEntryRepository;

	public CustomEntriesController(CustomEntryRepository customEntryRepository)
	{
		_customEntryRepository = customEntryRepository;
	}

	[HttpGet("{id}/replay")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetCustomEntryReplayById([Required] int id)
	{
		(string fileName, byte[] contents) = _customEntryRepository.GetCustomEntryReplayById(id);
		return File(contents, MediaTypeNames.Application.Octet, fileName);
	}
}
