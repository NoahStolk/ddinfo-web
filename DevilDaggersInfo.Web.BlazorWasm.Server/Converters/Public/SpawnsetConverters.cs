using DevilDaggersCore.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public
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
