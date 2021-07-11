using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Spawnsets;
using DevilDaggersWebsite.Entities;

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
	}
}
