namespace DevilDaggersInfo.Core.Asset;

public static class CoreShaders
{
	public static readonly IReadOnlyList<ShaderAssetInfo> All = new List<ShaderAssetInfo>
	{
		new("gui", false),
		new("guifont", false),
		new("guifontcolour", false),
		new("guifontlev", false),
		new("test", false),
	};
}
