namespace DevilDaggersInfo.Common.Utils;

public static class ArrayUtils
{
	public static bool AreEqual(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b)
	{
		if (a.Length != b.Length)
			return false;

		for (int i = 0; i < a.Length; i++)
		{
			if (a[i] != b[i])
				return false;
		}

		return true;
	}
}
