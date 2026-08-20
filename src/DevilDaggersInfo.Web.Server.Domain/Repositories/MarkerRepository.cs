using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Repositories;

public sealed class MarkerRepository(ApplicationDbContext dbContext)
{
	public async Task<long> GetMarkerAsync(string name)
	{
		MarkerEntity? marker = await dbContext.Markers.FirstOrDefaultAsync(m => m.Name == name);
		if (marker == null)
			throw new NotFoundException($"Marker key '{name}' was not found in database.");

		return marker.Value;
	}
}
