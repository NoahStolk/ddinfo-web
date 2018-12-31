using CoreBase.Services;
using DevilDaggersWebsite.Models.User;
using NetBase.Utils;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.Utils
{
	public static class UserUtils
	{
		public static IEnumerable<int> GetBans(ICommonObjects commonObjects)
		{
			foreach (string ban in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "bans")).Split('\n'))
				if (!string.IsNullOrWhiteSpace(ban))
					yield return int.Parse(ban.TrimEnd('\r', '\n'));
		}

		public static IEnumerable<Donator> GetDonators(ICommonObjects commonObjects)
		{
			foreach (string d in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "donators")).Split('\n'))
			{
				if (string.IsNullOrWhiteSpace(d))
					continue;

				string donator = d.TrimEnd('\r', '\n');
				string[] props = GetProps(donator);

				yield return new Donator(int.Parse(props[0]), props[1], int.Parse(props[2]), char.Parse(props[3]));
			}
		}

		public static IEnumerable<Flag> GetFlags(ICommonObjects commonObjects)
		{
			foreach (string f in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "flags")).Split('\n'))
			{
				if (string.IsNullOrWhiteSpace(f))
					continue;

				string flag = f.TrimEnd('\r', '\n');

				if (flag.EndsWith("?"))
					continue;

				string[] props = GetProps(flag);

				yield return new Flag(int.Parse(props[0]), props[1]);
			}
		}

		private static string[] GetProps(string line)
		{
			while (line.Contains("\t"))
				line = line.Replace("\t", " ");
			while (line.Contains("  "))
				line = line.Replace("  ", " ");
			return line.Split(' ');
		}
	}
}