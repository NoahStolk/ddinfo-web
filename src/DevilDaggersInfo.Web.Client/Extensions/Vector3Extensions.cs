using System.Numerics;

namespace DevilDaggersInfo.Web.Client.Extensions;

internal static class Vector3Extensions
{
	extension(Vector3 vector)
	{
		public string HexCode => $"#{GetComponent(vector.X):X2}{GetComponent(vector.Y):X2}{GetComponent(vector.Z):X2}";
	}

	private static byte GetComponent(float v)
	{
		return (byte)(v * byte.MaxValue);
	}
}
