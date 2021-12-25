namespace DevilDaggersInfo.Web.BlazorWasm.Server.Utils;

public static class PngFileUtils
{
	private const int _pngHeaderLength = 8;
	private static readonly byte[] _pngHeader = new byte[_pngHeaderLength] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

	public static bool HasValidPngHeader(byte[] bytes)
	{
		if (bytes.Length < _pngHeaderLength)
			return false;

		for (int j = 0; j < _pngHeaderLength; j++)
		{
			if (bytes[j] != _pngHeader[j])
				return false;
		}

		return true;
	}
}
