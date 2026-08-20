using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public sealed class MarkerService(ApplicationDbContext dbContext, ILogger<MarkerService> logger)
{
	public async Task EditMarkerAsync(string name, long value)
	{
		MarkerEntity? marker = await dbContext.Markers.FirstOrDefaultAsync(m => m.Name == name);
		if (marker == null)
			throw new NotFoundException();

		long oldValue = marker.Value;
		marker.Value = value;
		await dbContext.SaveChangesAsync();

		logger.LogInformation("Marker '{MarkerName}' was updated from '{Old}' to '{New}'.", marker.Name, $"0x{oldValue:X16}", $"0x{marker.Value:X16}");
	}
}
