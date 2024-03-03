// ReSharper disable StringLiteralTypo
namespace DevilDaggersInfo.Web.Server.Utils.AssetInfo;

public static class CoreShaders
{
	public static AssetInfoEntry[] All { get; } =
	[
		new("gui", "?", ["UI"]),
		new("guifont", "?", ["Font", "UI"]),
		new("guifontcolour", "?", ["Font", "UI"]),
		new("guifontlev", "?", ["Font", "UI"]),
		new("test", "?", ["?"]),
	];
}
