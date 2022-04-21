using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Editor.Asset.Components;

public partial class BinaryViewer
{
	private readonly List<AssetKey> _assets = new();

	public void ReadBinary(string fileName, byte[] fileContents)
	{
		ModBinary modBinary = new(fileName, fileContents, ModBinaryReadComprehensiveness.TocOnly);
		_assets.AddRange(modBinary.Chunks.Select(c => new AssetKey(c.AssetType, c.Name)));
	}
}
