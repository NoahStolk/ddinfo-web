using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.Mod.Exceptions;
using System.Text;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;

public class ModBinaryCacheData
{
	public ModBinaryCacheData(string name, long size, ModBinaryType modBinaryType, List<ModChunkCacheData> chunks, List<(string Name, bool IsProhibited)>? loudnessAssets)
	{
		Name = name;
		Size = size;
		ModBinaryType = modBinaryType;
		Chunks = chunks;
		LoudnessAssets = loudnessAssets;
	}

	public string Name { get; }
	public long Size { get; }
	public ModBinaryType ModBinaryType { get; }
	public List<ModChunkCacheData> Chunks { get; }
	public List<(string Name, bool IsProhibited)>? LoudnessAssets { get; }

	public static ModBinaryCacheData CreateFromFile(string fileName, byte[] fileContents)
	{
		ModBinary modBinary = ModBinaryHandler.ReadChunks(fileName, fileContents);

		List<ModChunkCacheData> chunks = modBinary.Chunks
			.ConvertAll(c =>
			{
				bool isProhibited = modBinary.ModBinaryType switch
				{
					ModBinaryType.Audio => AudioAudio.All.Find(a => a.AssetName == c.Name)?.IsProhibited ?? false,
					ModBinaryType.Core => CoreShaders.All.Find(a => a.AssetName == c.Name)?.IsProhibited ?? false,
					ModBinaryType.Dd => c.AssetType switch
					{
						AssetType.ModelBinding => DdModelBindings.All.Find(a => a.AssetName == c.Name)?.IsProhibited ?? false,
						AssetType.Model => DdModels.All.Find(a => a.AssetName == c.Name)?.IsProhibited ?? false,
						AssetType.Shader => DdShaders.All.Find(a => a.AssetName == c.Name)?.IsProhibited ?? false,
						AssetType.Texture => DdTextures.All.Find(a => a.AssetName == c.Name)?.IsProhibited ?? false,
						_ => throw new InvalidModBinaryException($"Binary '{fileName}', which is a '{modBinary.ModBinaryType}' binary file, contains an asset of type '{c.AssetType}', which is not supported."),
					},
					_ => throw new NotSupportedException($"Binary type '{modBinary.ModBinaryType}' is not supported."),
				};

				return new ModChunkCacheData(c.Name, c.Size, c.AssetType, isProhibited);
			});

		ModBinaryChunk? loudnessChunk = modBinary.Chunks.Find(c => c.AssetType == AssetType.Audio && c.Name == "loudness");
		List<(string Name, bool IsProhibited)>? loudnessAssets = null;
		if (loudnessChunk != null)
		{
			byte[] loudnessBytes = new byte[loudnessChunk.Size];
			Buffer.BlockCopy(fileContents, (int)loudnessChunk.Offset, loudnessBytes, 0, (int)loudnessChunk.Size);
			string loudnessString = Encoding.Default.GetString(loudnessBytes);
			loudnessAssets = ReadLoudness(loudnessString);
		}

		return new(fileName, fileContents.Length, modBinary.ModBinaryType, chunks, loudnessAssets);
	}

	private static List<(string Name, bool IsProhibited)> ReadLoudness(string loudnessString)
	{
		List<(string Name, bool IsProhibited)> loudnessAssets = new();

		foreach (string line in loudnessString.Split('\n'))
		{
			if (!TryReadLoudnessLine(line, out string? assetName, out float loudness) || assetName == null)
				continue;

			AudioAssetData? audioAssetData = AudioAudio.All.Find(a => a.AssetName == assetName);
			if (audioAssetData == null || audioAssetData.DefaultLoudness == loudness)
				continue;

			loudnessAssets.Add((assetName, audioAssetData.IsProhibited));
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

			assetName = line.Substring(0, equalsIndex);
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
}
