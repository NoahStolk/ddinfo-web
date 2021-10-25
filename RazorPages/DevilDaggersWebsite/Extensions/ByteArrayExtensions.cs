using System;

namespace DevilDaggersWebsite.Extensions
{
	public static class ByteArrayExtensions
	{
		public static string ByteArrayToHexString(this byte[] byteArray)
			=> BitConverter.ToString(byteArray).Replace("-", string.Empty);
	}
}
