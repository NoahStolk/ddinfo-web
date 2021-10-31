using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Titles;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Admin;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/titles")]
[ApiController]
public class TitlesController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly AuditLogger _auditLogger;

	public TitlesController(ApplicationDbContext dbContext, AuditLogger auditLogger)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Page<GetTitle>> GetTitles(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault,
		TitleSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<TitleEntity> titlesQuery = _dbContext.Titles.AsNoTracking();

		titlesQuery = sortBy switch
		{
			TitleSorting.Name => titlesQuery.OrderBy(s => s.Name, ascending),
			_ => titlesQuery.OrderBy(s => s.Id, ascending),
		};

		List<TitleEntity> titles = titlesQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetTitle>
		{
			Results = titles.ConvertAll(t => t.ToGetTitle()),
			TotalResults = _dbContext.Titles.Count(),
		};
	}

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetTitleName>> GetTitleNames()
	{
		var titles = _dbContext.Titles
			.AsNoTracking()
			.Select(t => new { t.Id, t.Name })
			.ToList();

		return titles.ConvertAll(p => new GetTitleName
		{
			Id = p.Id,
			Name = p.Name,
		});
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddTitle(AddTitle addTitle)
	{
		foreach (int playerId in addTitle.PlayerIds ?? new())
		{
			if (!_dbContext.Players.Any(p => p.Id == playerId))
				return BadRequest($"Player with ID '{playerId}' does not exist.");
		}

		TitleEntity title = new()
		{
			Name = addTitle.Name,
		};
		_dbContext.Titles.Add(title);
		_dbContext.SaveChanges(); // Save changes here so PlayerTitle entities can be assigned properly.

		UpdatePlayerTitles(addTitle.PlayerIds ?? new(), title.Id);
		_dbContext.SaveChanges();

		await _auditLogger.LogAdd(addTitle, User, title.Id);

		return Ok(title.Id);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditTitleById(int id, EditTitle editTitle)
	{
		foreach (int playerId in editTitle.PlayerIds ?? new())
		{
			if (!_dbContext.Players.Any(p => p.Id == playerId))
				return BadRequest($"Player with ID '{playerId}' does not exist.");
		}

		TitleEntity? title = _dbContext.Titles
			.Include(t => t.PlayerTitles)
			.FirstOrDefault(t => t.Id == id);
		if (title == null)
			return NotFound();

		EditTitle logDto = new()
		{
			Name = title.Name,
			PlayerIds = title.PlayerTitles.ConvertAll(pt => pt.PlayerId),
		};

		title.Name = editTitle.Name;

		UpdatePlayerTitles(editTitle.PlayerIds ?? new(), title.Id);
		_dbContext.SaveChanges();

		await _auditLogger.LogEdit(logDto, editTitle, User, title.Id);

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteTitleById(int id)
	{
		TitleEntity? title = _dbContext.Titles
			.Include(t => t.PlayerTitles)
			.FirstOrDefault(t => t.Id == id);
		if (title == null)
			return NotFound();

		_dbContext.Titles.Remove(title);
		_dbContext.SaveChanges();

		await _auditLogger.LogDelete(title, User, title.Id);

		return Ok();
	}

	private void UpdatePlayerTitles(List<int> playerIds, int titleId)
	{
		foreach (PlayerTitleEntity newEntity in playerIds.ConvertAll(pi => new PlayerTitleEntity { TitleId = titleId, PlayerId = pi }))
		{
			if (!_dbContext.PlayerTitles.Any(pt => pt.TitleId == newEntity.TitleId && pt.PlayerId == newEntity.PlayerId))
				_dbContext.PlayerTitles.Add(newEntity);
		}

		foreach (PlayerTitleEntity entityToRemove in _dbContext.PlayerTitles.Where(pt => pt.TitleId == titleId && !playerIds.Contains(pt.PlayerId)))
			_dbContext.PlayerTitles.Remove(entityToRemove);
	}
}
