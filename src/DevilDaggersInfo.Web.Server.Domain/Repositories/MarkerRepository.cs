using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Repositories;

public class MarkerRepository
{
	private readonly ApplicationDbContext _dbContext;

	public MarkerRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<long> GetMarkerAsync(string name)
	{
		MarkerEntity? marker = await _dbContext.Markers.FirstOrDefaultAsync(m => m.Name == name);
		if (marker == null)
			throw new NotFoundException($"Marker key '{name}' was not found in database.");

		return marker.Value;
	}
}
