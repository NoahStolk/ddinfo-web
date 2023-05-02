namespace DevilDaggersInfo.App.Engine.Content;

public class Charset // TODO: Remove.
{
	public Charset(byte[] binaryContents)
	{
		string raw = Encoding.UTF8.GetString(binaryContents);
		Characters = raw.Replace("\r", string.Empty).Replace("\n", string.Empty); // Newlines must be removed.
	}

	public string Characters { get; }
}
