using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/custom-entries")]
[ApiController]
public class CustomEntriesController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly CustomEntryRepository _customEntryRepository;

	public CustomEntriesController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, CustomEntryRepository customEntryRepository)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_customEntryRepository = customEntryRepository;
	}

	[HttpGet("{id}/replay-buffer")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)] // TODO: Remove incorrect response type FileContentResult.
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<byte[]>> GetCustomEntryReplayBufferById([Required] int id)
	{
		return await _customEntryRepository.GetCustomEntryReplayBufferByIdAsync(id);
	}

	// FORBIDDEN: Used by DDLIVE.
	// FORBIDDEN: Used by DDCL 1.8.3.0.
	[HttpGet("{id}/replay")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetCustomEntryReplayById([Required] int id)
	{
		(string fileName, byte[] contents) = await _customEntryRepository.GetCustomEntryReplayByIdAsync(id);
		return File(contents, MediaTypeNames.Application.Octet, fileName);
	}

	[HttpGet("{id}/data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetCustomEntryData> GetCustomEntryDataById([Required] int id)
	{
		// ! Navigation property.
		CustomEntryEntity? customEntry = _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.Player)
			.Include(ce => ce.CustomLeaderboard)
				.ThenInclude(cl => cl!.Spawnset)
			.FirstOrDefault(cl => cl.Id == id);
		if (customEntry == null)
			return NotFound();

		CustomEntryDataEntity? customEntryData = _dbContext.CustomEntryData
			.AsNoTracking()
			.FirstOrDefault(ced => ced.CustomEntryId == id);

		// ! Navigation property.
		return customEntry.ToMainApi(customEntryData, customEntry.CustomLeaderboard!.Spawnset!.EffectiveHandLevel, IoFile.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay")));
	}
}
