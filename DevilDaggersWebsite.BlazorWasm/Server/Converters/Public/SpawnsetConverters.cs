using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Spawnsets;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Public
{
	public static class SpawnsetConverters
	{
		public static GetSpawnset ToGetSpawnsetPublic(this SpawnsetEntity spawnsetFile, SpawnsetData spawnsetData, bool hasCustomLeaderboard) => new()
		{
			MaxDisplayWaves = spawnsetFile.MaxDisplayWaves,
			HtmlDescription = spawnsetFile.HtmlDescription,
			LastUpdated = spawnsetFile.LastUpdated,
			SpawnsetData = spawnsetData,
			Name = spawnsetFile.Name,
			AuthorName = spawnsetFile.Player.PlayerName,
			HasCustomLeaderboard = hasCustomLeaderboard,
			IsPractice = spawnsetFile.IsPractice,
		};
	}
}
