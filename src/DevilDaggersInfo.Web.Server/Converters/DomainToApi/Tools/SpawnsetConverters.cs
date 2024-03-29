using DevilDaggersInfo.Web.Server.Domain.Entities;
using ToolsApi = DevilDaggersInfo.Web.ApiSpec.Tools.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Tools;

// TODO: Use domain models.
public static class SpawnsetConverters
{
	public static ToolsApi.GetSpawnset ToAppApi(this SpawnsetEntity spawnset, int? customLeaderboardId, byte[] fileBytes)
	{
		// ! Navigation property.
		return new()
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
}
