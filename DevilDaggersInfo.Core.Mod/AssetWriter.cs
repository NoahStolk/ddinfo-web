namespace DevilDaggersInfo.Core.Mod;

public static class AssetWriter
{
	public static void WriteFile(string outputDirectory, ModBinaryChunk chunk, byte[] buffer)
	{
		FileResult result = chunk.AssetType switch
		{
			AssetType.Audio => new("wav", buffer),
			AssetType.Model => new("obj", ObjFileHandler.Instance.ToFile(buffer)),
			_ => throw new NotSupportedException($"Creating file of type '{chunk.AssetType}' is not supported."),
		};
		File.WriteAllBytes(Path.Combine(outputDirectory, $"{chunk.Name}.{result.Extension}"), result.Buffer);
	}

	private record FileResult(string Extension, byte[] Buffer);
}
