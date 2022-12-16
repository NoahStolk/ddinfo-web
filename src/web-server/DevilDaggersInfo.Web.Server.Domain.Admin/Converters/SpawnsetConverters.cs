using DevilDaggersInfo.Api.Admin.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Extensions;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class SpawnsetConverters
{
	// ! Navigation property.
	public static GetSpawnsetForOverview ToGetSpawnsetForOverview(this SpawnsetEntity spawnset) => new()
	{
		Id = spawnset.Id,
		Author = spawnset.Player!.PlayerName,
		Name = spawnset.Name,
		MaxDisplayWaves = spawnset.MaxDisplayWaves,
		HtmlDescription = spawnset.HtmlDescription?.TrimAfter(40, true),
		LastUpdated = spawnset.LastUpdated,
		IsPractice = spawnset.IsPractice,
	};

	public static GetSpawnset ToGetSpawnset(this SpawnsetEntity spawnset) => new()
	{
		Id = spawnset.Id,
		PlayerId = spawnset.PlayerId,
		Name = spawnset.Name,
		MaxDisplayWaves = spawnset.MaxDisplayWaves,
		HtmlDescription = spawnset.HtmlDescription,
		LastUpdated = spawnset.LastUpdated,
		IsPractice = spawnset.IsPractice,
	};
}
