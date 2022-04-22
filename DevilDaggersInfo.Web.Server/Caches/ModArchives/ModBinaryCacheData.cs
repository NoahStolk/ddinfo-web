using DevilDaggersInfo.Core.Mod.Utils;

namespace DevilDaggersInfo.Web.Server.Caches.ModArchives;

public class ModBinaryCacheData
{
	public ModBinaryCacheData(string name, long size, ModBinaryType modBinaryType, List<ModChunkCacheData> chunks, List<ModifiedLoudnessAssetCacheData>? modifiedLoudnessAssets)
	{
		Name = name;
		Size = size;
		ModBinaryType = modBinaryType;
		Chunks = chunks;
		ModifiedLoudnessAssets = modifiedLoudnessAssets;
	}

	public string Name { get; }
	public long Size { get; }
	public ModBinaryType ModBinaryType { get; }
	public List<ModChunkCacheData> Chunks { get; }
	public List<ModifiedLoudnessAssetCacheData>? ModifiedLoudnessAssets { get; }

	public static ModBinaryCacheData CreateFromFile(string fileName, byte[] fileContents)
	{
		ModBinaryType binaryTypeFromFileName = BinaryFileNameUtils.GetBinaryTypeBasedOnFileName(fileName);

		ModBinary modBinary = new(fileContents, ModBinaryReadComprehensiveness.TocAndLoudness);
		if (modBinary.ModBinaryType != binaryTypeFromFileName)
			throw new InvalidModBinaryException($"Binary '{fileName}' has type mismatch; file name claims '{binaryTypeFromFileName}' but file contents claim '{modBinary.ModBinaryType}'.");

		List<ModChunkCacheData> chunks = modBinary.Chunks.ConvertAll(c => new ModChunkCacheData(c.Name, c.Size, c.AssetType, AssetContainer.GetIsProhibited(c.AssetType, c.Name)));

		ModBinaryChunk? loudnessChunk = modBinary.Chunks.Find(c => c.IsLoudness());
		List<ModifiedLoudnessAssetCacheData>? modifiedLoudnessAssets = null;
		if (loudnessChunk != null)
		{
			byte[] loudnessBytes = new byte[loudnessChunk.Size];
			Buffer.BlockCopy(fileContents, loudnessChunk.Offset, loudnessBytes, 0, loudnessChunk.Size);
			string loudnessString = Encoding.Default.GetString(loudnessBytes);
			modifiedLoudnessAssets = ReadModifiedLoudnessValues(loudnessString);
		}

		return new(fileName, fileContents.Length, modBinary.ModBinaryType, chunks, modifiedLoudnessAssets);
	}

	private static List<ModifiedLoudnessAssetCacheData> ReadModifiedLoudnessValues(string loudnessString)
	{
		List<ModifiedLoudnessAssetCacheData> loudnessAssets = new();

		foreach (string line in loudnessString.Split('\n'))
		{
			if (!TryReadLoudnessLine(line, out string? assetName, out float loudness) || assetName == null)
				continue;

			AudioAssetData? audioAssetData = AudioAudio.All.Find(a => a.AssetName == assetName);
			if (audioAssetData == null || audioAssetData.DefaultLoudness == loudness)
				continue;

			loudnessAssets.Add(new(assetName, audioAssetData.IsProhibited, audioAssetData.DefaultLoudness, loudness));
		}

		return loudnessAssets;
	}

	private static bool TryReadLoudnessLine(string line, out string? assetName, out float loudness)
	{
		try
		{
			line = line
				.Replace(" ", string.Empty, StringComparison.InvariantCulture) // Remove spaces to make things easier.
				.TrimEnd('.'); // Remove dots at the end of the line. (The original loudness file has one on line 154 for some reason...)

			int equalsIndex = line.IndexOf('=', StringComparison.InvariantCulture);

			assetName = line[..equalsIndex];
			loudness = float.Parse(line.Substring(equalsIndex + 1, line.Length - assetName.Length - 1));
			return true;
		}
		catch
		{
			assetName = null;
			loudness = 0;
			return false;
		}
	}

	public bool ContainsProhibitedAssets()
		=> Chunks.Any(mccd => mccd.IsProhibited);
}
