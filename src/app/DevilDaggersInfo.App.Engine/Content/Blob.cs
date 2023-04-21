namespace Warp.NET.Content;

public class Blob
{
	public Blob(byte[] data)
	{
		Data = data;
	}

	public byte[] Data { get; }
}
