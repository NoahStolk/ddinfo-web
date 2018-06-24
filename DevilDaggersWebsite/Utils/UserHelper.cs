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
			72195
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
			new Donator("LocoCaesar_IV", 1200, '€'),
			new Donator("LukeNukem", 1001, '€'),
			new Donator("Stop.", 751, '€'),
			new Donator("Zirtonic", 500, '$'),
			new Donator("Satan", 500, '$'),
			new Donator("DJDoomz", 100, '€'),
			new Donator("Chupacabra", 100, '€'),
			new Donator(".curry", 77, '€')
		};
	}
}