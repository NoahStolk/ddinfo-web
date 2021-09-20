namespace DevilDaggersInfo.Core.Shared.Extensions;

public static class EnumExtensions
{
	public static IEnumerable<int> AsEnumerable<TEnum>(this TEnum e)
		where TEnum : struct, Enum
	{
		int count = Enum.GetValues(typeof(TEnum)).Length;
		int max = 1 << (count - 1);

		for (int i = 1; i < max; i <<= 1)
		{
			if (((int)(object)e & i) != 0)
				yield return i;
		}
	}

	public static TEnum ToFlagEnum<TEnum>(this List<int> list)
		where TEnum : Enum
		=> (TEnum)(object)list.Sum();
}
