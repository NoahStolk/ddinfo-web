using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Exceptions;
using DevilDaggersInfo.Core.Mod.Utils;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

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

		ModBinary modBinary = new(fileContents, ModBinaryReadFilter.Assets(new AssetKey(AssetType.Audio, "loudness")));
		if (modBinary.Toc.Type != binaryTypeFromFileName)
			throw new InvalidModBinaryException($"Binary '{fileName}' has type mismatch; file name claims '{binaryTypeFromFileName}' but file contents claim '{modBinary.Toc.Type}'.");

		List<ModChunkCacheData> chunks = modBinary.Toc.Chunks.Select(c => new ModChunkCacheData
		{
			Name = c.Name,
			Size = c.Size,
			AssetType = c.AssetType,
			IsProhibited = AssetContainer.GetIsProhibited(c.AssetType, c.Name),
		}).ToList();

		ModBinaryChunk? loudnessChunk = modBinary.Toc.Chunks.FirstOrDefault(c => c.IsLoudness());
		List<ModifiedLoudnessAssetCacheData>? modifiedLoudnessAssets = null;
		if (loudnessChunk != null)
		{
			byte[] loudnessBytes = new byte[loudnessChunk.Size];
			Buffer.BlockCopy(fileContents, loudnessChunk.Offset, loudnessBytes, 0, loudnessChunk.Size);
			string loudnessString = Encoding.UTF8.GetString(loudnessBytes);
			modifiedLoudnessAssets = ReadModifiedLoudnessValues(loudnessString);
		}

		return new(fileName, fileContents.Length, modBinary.Toc.Type, chunks, modifiedLoudnessAssets);
	}

	private static List<ModifiedLoudnessAssetCacheData> ReadModifiedLoudnessValues(string loudnessString)
	{
		List<ModifiedLoudnessAssetCacheData> loudnessAssets = new();

		foreach (string line in loudnessString.Split('\n'))
		{
			if (!TryReadLoudnessLine(line, out string? assetName, out float loudness) || assetName == null)
				continue;

			AudioAssetInfo? audioAssetData = AudioAudio.All.Find(a => a.AssetName == assetName);
			if (audioAssetData == null || Math.Abs(audioAssetData.DefaultLoudness - loudness) < 0.01f)
				continue;

			loudnessAssets.Add(new ModifiedLoudnessAssetCacheData
			{
				Name = assetName,
				DefaultLoudness = audioAssetData.DefaultLoudness,
				IsProhibited = audioAssetData.IsProhibited,
				ModifiedLoudness = loudness,
			});
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
		=> Chunks.Any(c => c.IsProhibited);
}
