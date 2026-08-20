using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DevilDaggersInfo.Web.Server.Controllers.DdLive;

[Route("api/ddlive/custom-entries")]
[ApiController]
public sealed class CustomEntriesController(CustomEntryRepository customEntryRepository) : ControllerBase
{
	[HttpGet("{id}/replay")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetCustomEntryReplayById([Required] int id)
	{
		(string fileName, byte[] contents) = await customEntryRepository.GetCustomEntryReplayByIdAsync(id);
		return File(contents, MediaTypeNames.Application.Octet, fileName);
	}
}
