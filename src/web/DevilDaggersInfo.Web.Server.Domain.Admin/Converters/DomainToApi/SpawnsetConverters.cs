using DevilDaggersInfo.Web.ApiSpec.Admin.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Extensions;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;

public static class SpawnsetConverters
{
	public static GetSpawnsetForOverview ToAdminApiOverview(this SpawnsetEntity spawnset)
	{
		if (spawnset.Player == null)
			throw new InvalidOperationException("Player is not included.");

		return new()
		{
			Id = spawnset.Id,
			Author = spawnset.Player.PlayerName,
			Name = spawnset.Name,
			MaxDisplayWaves = spawnset.MaxDisplayWaves,
			HtmlDescription = spawnset.HtmlDescription?.TrimAfter(40, true),
			LastUpdated = spawnset.LastUpdated,
			IsPractice = spawnset.IsPractice,
		};
	}

	public static GetSpawnset ToAdminApi(this SpawnsetEntity spawnset) => new()
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
