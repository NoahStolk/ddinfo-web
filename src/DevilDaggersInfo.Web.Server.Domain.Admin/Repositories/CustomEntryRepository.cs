using DevilDaggersInfo.Web.ApiSpec.Admin.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public class CustomEntryRepository
{
	private readonly ApplicationDbContext _dbContext;

	public CustomEntryRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<GetCustomEntryForOverview>> GetCustomEntriesAsync(CancellationToken cancellationToken)
	{
		// ! Navigation property.
		List<CustomEntryEntity> customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.Player)
			.Include(ce => ce.CustomLeaderboard)
				.ThenInclude(cl => cl!.Spawnset)
			.ToListAsync(cancellationToken);

		return customEntries.ConvertAll(ce => ce.ToAdminApiOverview());
	}

	public async Task<GetCustomEntry> GetCustomEntryAsync(int id)
	{
		CustomEntryEntity? customEntry = await _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.CustomLeaderboard)
			.FirstOrDefaultAsync(cl => cl.Id == id);

		if (customEntry == null)
			throw new NotFoundException();

		return customEntry.ToAdminApi();
	}
}
