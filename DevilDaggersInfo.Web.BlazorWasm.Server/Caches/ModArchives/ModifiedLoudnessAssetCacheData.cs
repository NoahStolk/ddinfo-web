namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;

public class ModifiedLoudnessAssetCacheData
{
	public ModifiedLoudnessAssetCacheData(string name, bool isProhibited)
	{
		Name = name;
		IsProhibited = isProhibited;
	}

	public string Name { get; }
	public bool IsProhibited { get; }
}
