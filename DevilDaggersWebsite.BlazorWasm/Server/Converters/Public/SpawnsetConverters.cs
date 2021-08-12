using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Spawnsets;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Public
{
	public static class SpawnsetConverters
	{
		public static GetSpawnset ToGetSpawnsetPublic(this SpawnsetEntity spawnset, SpawnsetData spawnsetData, bool hasCustomLeaderboard) => new()
		{
			MaxDisplayWaves = spawnset.MaxDisplayWaves,
			HtmlDescription = spawnset.HtmlDescription,
			LastUpdated = spawnset.LastUpdated,
			SpawnsetData = spawnsetData,
			Name = spawnset.Name,
			AuthorName = spawnset.Player.PlayerName,
			HasCustomLeaderboard = hasCustomLeaderboard,
			IsPractice = spawnset.IsPractice,
		};
	}
}
