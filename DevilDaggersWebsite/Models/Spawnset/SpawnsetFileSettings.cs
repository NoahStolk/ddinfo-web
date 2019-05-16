using DevilDaggersWebsite.Utils;
using Newtonsoft.Json;
using System;

namespace DevilDaggersWebsite.Models.Spawnset
{
	[JsonObject(MemberSerialization.OptIn)]
	public class SpawnsetFileSettings
	{
		[JsonProperty]
		public int MaxWaves { get; set; } = RazorUtils.DEFAULT_MAX_WAVES;
		[JsonProperty]
		public string Description { get; set; }
		[JsonProperty]
		public DateTime LastUpdated { get; set; }

		public SpawnsetFileSettings(int maxWaves, string description, DateTime lastUpdated)
		{
			MaxWaves = maxWaves;
			Description = description;
			LastUpdated = lastUpdated;
		}

		public SpawnsetFileSettings()
		{
		}
	}
}