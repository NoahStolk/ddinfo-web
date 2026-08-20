using DevilDaggersInfo.Web.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public sealed class MarkerRepository(ApplicationDbContext dbContext)
{
	public async Task<List<string>> GetMarkerNamesAsync()
	{
		return await dbContext.Markers.AsNoTracking().Select(m => m.Name).ToListAsync();
	}
}
