using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class MarkerService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ILogger<MarkerService> _logger;

	public MarkerService(ApplicationDbContext dbContext, ILogger<MarkerService> logger)
	{
		_dbContext = dbContext;
		_logger = logger;
	}

	public async Task EditMarkerAsync(string name, long value)
	{
		MarkerEntity? marker = await _dbContext.Markers.FirstOrDefaultAsync(m => m.Name == name);
		if (marker == null)
			throw new NotFoundException();

		long oldValue = marker.Value;
		marker.Value = value;
		await _dbContext.SaveChangesAsync();

		_logger.LogWarning("Marker '{markerName}' was updated from '{old}' to '{new}'.", marker.Name, $"0x{oldValue:X16}", $"0x{marker.Value:X16}");
	}
}
