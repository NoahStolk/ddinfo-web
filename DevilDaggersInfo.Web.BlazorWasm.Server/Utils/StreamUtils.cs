namespace DevilDaggersInfo.Web.BlazorWasm.Server.Utils;

public static class StreamUtils
{
	/// <summary>
	/// Reading the entire underlying stream from a <see cref="ZipArchiveEntry"/> does not always read all bytes present. This method keeps reading until everything is read.
	/// </summary>
	public static int ForceReadAllBytes(Stream stream, byte[] buffer, int offset, int count)
	{
		int totalBytesRead = 0;
		while (totalBytesRead < count)
		{
			int bytesRead = stream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
			if (bytesRead == 0)
				throw new IOException("Premature end of stream.");

			totalBytesRead += bytesRead;
		}

		return totalBytesRead;
	}
}
