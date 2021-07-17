using DevilDaggersCore.Extensions;
using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Spawnsets;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters
{
	public static class SpawnsetConverters
	{
		public static GetSpawnset ToGetSpawnset(this SpawnsetFile spawnset) => new()
		{
			Id = spawnset.Id,
			PlayerId = spawnset.PlayerId,
			Name = spawnset.Name,
			MaxDisplayWaves = spawnset.MaxDisplayWaves,
			HtmlDescription = spawnset.HtmlDescription?.TrimAfter(40, true),
			LastUpdated = spawnset.LastUpdated,
			IsPractice = spawnset.IsPractice,
		};

		public static GetSpawnsetPublic ToGetSpawnsetPublic(this SpawnsetFile spawnsetFile, SpawnsetData spawnsetData, bool hasCustomLeaderboard) => new()
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
