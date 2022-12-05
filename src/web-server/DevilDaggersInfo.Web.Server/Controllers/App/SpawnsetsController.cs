using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using GetSpawnsetBuffer = DevilDaggersInfo.Api.Ddcl.Spawnsets.GetSpawnsetBuffer;

namespace DevilDaggersInfo.Web.Server.Controllers.App;

[Route("api/app/spawnsets")]
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

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetSpawnset> GetSpawnsetById([Required] int id)
	{
		SpawnsetEntity? spawnsetEntity = _dbContext.Spawnsets
			.AsNoTracking()
			.Include(s => s.Player)
			.FirstOrDefault(s => s.Id == id);
		if (spawnsetEntity == null)
			return NotFound();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnsetEntity.Name);
		if (!IoFile.Exists(path))
			return NotFound();

		var customLeaderboard = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Select(cl => new { cl.Id, cl.SpawnsetId })
			.FirstOrDefault(cl => cl.SpawnsetId == spawnsetEntity.Id);

		return spawnsetEntity.ToGetSpawnset(customLeaderboard?.Id, IoFile.ReadAllBytes(path));
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
			Data = await IoFile.ReadAllBytesAsync(path),
		};
	}
}
