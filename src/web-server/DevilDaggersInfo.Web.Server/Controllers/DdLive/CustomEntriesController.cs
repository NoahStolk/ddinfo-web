using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

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
	public async Task<ActionResult> GetCustomEntryReplayById([Required] int id)
	{
		(string fileName, byte[] contents) = await _customEntryRepository.GetCustomEntryReplayByIdAsync(id);
		return File(contents, MediaTypeNames.Application.Octet, fileName);
	}
}
