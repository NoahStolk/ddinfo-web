namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;

public class ModifiedLoudnessAssetCacheData
{
	public ModifiedLoudnessAssetCacheData(string name, bool isProhibited, float defaultLoudness, float modifiedLoudness)
	{
		Name = name;
		IsProhibited = isProhibited;
		DefaultLoudness = defaultLoudness;
		ModifiedLoudness = modifiedLoudness;
	}

	public string Name { get; }
	public bool IsProhibited { get; }
	public float DefaultLoudness { get; }
	public float ModifiedLoudness { get; }
}
