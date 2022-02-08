namespace DevilDaggersInfo.Core.Mod;

public class ModBinaryCompiler
{
	private readonly List<Asset> _assets = new();

	public ModBinaryCompiler(ModBinaryType modBinaryType)
	{
		if (!(modBinaryType is ModBinaryType.Audio or ModBinaryType.Dd))
			throw new NotSupportedException($"Compiling mods of type '{modBinaryType}' is not supported.");

		ModBinaryType = modBinaryType;
	}

	public ModBinaryType ModBinaryType { get; }

	public void AddAsset(string fileName, AssetType assetType, byte[] fileContents)
	{
		if (assetType == AssetType.Audio && ModBinaryType != ModBinaryType.Audio)
			throw new InvalidModCompilationException($"Cannot add an audio asset to a mod of type '{ModBinaryType}'.");
		else if (assetType != AssetType.Audio && ModBinaryType == ModBinaryType.Audio)
			throw new InvalidModCompilationException("Cannot add a non-audio asset to an audio mod.");

		_assets.Add(new(fileName, assetType, fileContents));
	}

	public byte[] Compile()
	{
		
	}

	private sealed class Asset
	{
		public Asset(string name, AssetType type, byte[] contents)
		{
			Name = name;
			Type = type;
			Contents = contents;
		}

		public string Name { get; }

		public AssetType Type { get; }

		public byte[] Contents { get; }
	}
}
