using DevilDaggersWebsite.Models.User;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Utils
{
	public static class UserUtils
	{
		public static List<int> BanIDs { get; set; } = new List<int>
		{
			117558,
			11356,
			87056,
			10217,
			80584,
			101345,
			129990,
			155788,
			117147,
			144104,
			157043,
			68182,
			160371,
			999999,
			10635,
			110997,
			90515,
			157181,
			161160,
			153152,
			150850,
			140942,
			76010,
			9666,
			72195,
			154532,
			75575,
			42534,
			106692,
			164240,
			21541,
			171883,
			170176,
			54358,
			176591,
			176744,
			121293,
			93520,
			108947
		};

		public static List<Donator> Donators { get; set; } = new List<Donator>
		{
			new Donator(137044, "DJDoomz", 100, '€'),
			new Donator(105315, "LukeNukem", 1001, '€'),
			new Donator(94857, ".curry", 77, '€'),
			new Donator(142939, "LocoCaesar_IV", 1200, '€'),
			new Donator(118832, "Chupacabra", 501, '€'),
			new Donator(113530, "Zirtonic", 1500, '$'),
			new Donator(148518, "Dillon", 500, '$'),
			new Donator(148951, "Stop.", 1500, '€'),
			new Donator(172395, "Tileä", 500, '$'),
			new Donator(115431, "Pritster", 500, '£'),
			new Donator(6760, "TSTAB", 1000, '€'),
			new Donator(134802, "gLad", 500, '€'),
			new Donator(65617, "pagedMov", 2500, '€'),
			new Donator(121891, "xamide", 500, '€'),
			new Donator(58491, "Green", 100, '$')
		};
	}
}