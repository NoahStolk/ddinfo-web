namespace DevilDaggersInfo.Common.Extensions;

public static class ByteArrayExtensions
{
	// TODO: Move to custom entry validation logic.
	public static string ByteArrayToHexString(this byte[] byteArray)
		=> BitConverter.ToString(byteArray).Replace("-", string.Empty);
}
