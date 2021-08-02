using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Spawnsets;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin
{
	public static class SpawnsetConverters
	{
		public static GetSpawnsetForOverview ToGetSpawnsetForOverview(this SpawnsetFile spawnset) => new()
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
