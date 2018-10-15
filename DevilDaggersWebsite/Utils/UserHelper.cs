using DevilDaggersWebsite.Models;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Utils
{
	public static class UserHelper
	{
		public static int AdminID { get; set; } = 21854;

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
			176591
		};

		public static List<int> DonatorIDs { get; set; } = new List<int>
		{
			142939,
			105315,
			113530,
			148518,
			137044,
			118832,
			94857
		};

		public static List<Donator> Donators { get; set; } = new List<Donator>
		{
			new Donator("DJDoomz", 100, '€'),
			new Donator("LukeNukem", 1001, '€'),
			new Donator(".curry", 77, '€'),
			new Donator("LocoCaesar_IV", 1200, '€'),
			new Donator("Chupacabra", 250, '€'),
			new Donator("Zirtonic", 1000, '$'),
			new Donator("Dillon", 500, '$'),
			new Donator("Stop.", 751, '€'),
			new Donator("Tileä", 500, '$'),
			new Donator("Pritster", 500, '£'),
			new Donator("TSTAB", 1000, '€'),
			new Donator("gLad", 500, '€')
		};
	}
}