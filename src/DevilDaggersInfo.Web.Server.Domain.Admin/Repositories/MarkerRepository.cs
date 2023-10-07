using DevilDaggersInfo.Web.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public class MarkerRepository
{
	private readonly ApplicationDbContext _dbContext;

	public MarkerRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<string>> GetMarkerNamesAsync()
	{
		return await _dbContext.Markers.AsNoTracking().Select(m => m.Name).ToListAsync();
	}
}
