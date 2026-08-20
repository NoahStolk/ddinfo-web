using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public sealed class ModRepository(ApplicationDbContext dbContext, ModArchiveAccessor modArchiveAccessor)
{
	public async Task<Page<GetModForOverview>> GetModsAsync(string? filter, int pageIndex, int pageSize, ModSorting? sortBy, bool ascending)
	{
		IQueryable<ModEntity> modsQuery = dbContext.Mods.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(filter))
		{
			filter = filter.Trim();
			modsQuery = modsQuery.Where(m => m.Name.Contains(filter));
		}

		modsQuery = sortBy switch
		{
			ModSorting.ModTypes => modsQuery.OrderBy(m => m.ModTypes, ascending).ThenBy(m => m.Id),
			ModSorting.HtmlDescription => modsQuery.OrderBy(m => m.HtmlDescription, ascending).ThenBy(m => m.Id),
			ModSorting.IsHidden => modsQuery.OrderBy(m => m.IsHidden, ascending).ThenBy(m => m.Id),
			ModSorting.LastUpdated => modsQuery.OrderBy(m => m.LastUpdated, ascending).ThenBy(m => m.Id),
			ModSorting.Name => modsQuery.OrderBy(m => m.Name, ascending).ThenBy(m => m.Id),
			ModSorting.TrailerUrl => modsQuery.OrderBy(m => m.TrailerUrl, ascending).ThenBy(m => m.Id),
			ModSorting.Url => modsQuery.OrderBy(m => m.Url, ascending).ThenBy(m => m.Id),
			_ => modsQuery.OrderBy(m => m.Id, ascending),
		};

		List<ModEntity> mods = await modsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return new Page<GetModForOverview>
		{
			Results = mods.ConvertAll(m => m.ToAdminApi()),
			TotalResults = await modsQuery.CountAsync(),
		};
	}

	public async Task<List<GetModName>> GetModNamesAsync()
	{
		var mods = await dbContext.Mods
			.AsNoTracking()
			.Select(m => new { m.Id, m.Name })
			.ToListAsync();

		return mods.ConvertAll(m => new GetModName
		{
			Id = m.Id,
			Name = m.Name,
		});
	}

	public async Task<GetMod> GetModAsync(int id)
	{
		ModEntity? mod = await dbContext.Mods
			.AsNoTracking()
			.Include(m => m.PlayerMods)
			.FirstOrDefaultAsync(m => m.Id == id);
		if (mod == null)
			throw new NotFoundException();

		ModFileSystemData fileSystemData = await modArchiveAccessor.GetModFileSystemDataAsync(mod.Name);
		return mod.ToAdminApi(fileSystemData.ModArchive?.Binaries.ConvertAll(b => b.Name), fileSystemData.ScreenshotFileNames);
	}
}
