using DevilDaggersInfo.Api.Ddcl.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;

	public SpawnsetsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
	}

	[HttpGet("{id}/buffer")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetSpawnsetBuffer>> GetSpawnsetBufferById([Required] int id)
	{
		var spawnset = await _dbContext.Spawnsets
			.AsNoTracking()
			.Select(s => new { s.Id, s.Name })
			.FirstOrDefaultAsync(s => s.Id == id);
		if (spawnset == null)
			throw new NotFoundException();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnset.Name);
		if (!IoFile.Exists(path))
			throw new NotFoundException();

		return new GetSpawnsetBuffer
		{
			Data = IoFile.ReadAllBytes(path),
		};
	}
}
