namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class FileSizeUtils
{
	public static string Format(int fileSizeInBytes)
	{
		if (fileSizeInBytes >= 1_000_000_000)
			return $"{fileSizeInBytes / 1_000_000_000f:0.##} GB";

		if (fileSizeInBytes >= 1_000_000)
			return $"{fileSizeInBytes / 1_000_000f:0.##} MB";

		if (fileSizeInBytes >= 1000)
			return $"{fileSizeInBytes / 1000f:0.##} KB";

		return $"{fileSizeInBytes} bytes";
	}
}
