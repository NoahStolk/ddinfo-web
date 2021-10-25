using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Spawnsets;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;

public static class SpawnsetConverters
{
	public static GetSpawnsetForOverview ToGetSpawnsetForOverview(this SpawnsetEntity spawnset) => new()
	{
		Id = spawnset.Id,
		Author = spawnset.Player.PlayerName,
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
