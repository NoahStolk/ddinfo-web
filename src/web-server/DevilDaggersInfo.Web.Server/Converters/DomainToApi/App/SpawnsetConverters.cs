using AppApi = DevilDaggersInfo.Api.App.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;

// TODO: Use domain models.
public static class SpawnsetConverters
{
	// ! Navigation property.
	public static AppApi.GetSpawnset ToGetSpawnset(this SpawnsetEntity spawnset, int? customLeaderboardId, byte[] fileBytes) => new()
	{
		AuthorName = spawnset.Player!.PlayerName,
		FileBytes = fileBytes,
		Id = spawnset.Id,
		IsPractice = spawnset.IsPractice,
		LastUpdated = spawnset.LastUpdated,
		Name = spawnset.Name,
		CustomLeaderboardId = customLeaderboardId,
		HtmlDescription = spawnset.HtmlDescription,
		MaxDisplayWaves = spawnset.MaxDisplayWaves,
	};
}
