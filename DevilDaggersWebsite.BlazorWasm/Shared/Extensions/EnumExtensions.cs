using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Extensions
{
	public static class EnumExtensions
	{
		public static IEnumerable<int> AsEnumerable<TEnum>(this TEnum e)
			where TEnum : Enum
		{
			for (int i = 1; i < 128; i *= 2)
			{
				if (((int)(object)e & i) != 0)
					yield return i;
			}
		}

		public static TEnum ToFlagEnum<TEnum>(this List<int> list)
			where TEnum : Enum
			=> (TEnum)(object)list.Sum();
	}
}
