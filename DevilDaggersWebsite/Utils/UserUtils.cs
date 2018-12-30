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
				yield return int.Parse(ban.TrimEnd('\r', '\n'));
		}

		public static IEnumerable<Donator> GetDonators(ICommonObjects commonObjects)
		{
			foreach (string d in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "donators")).Split('\n'))
			{
				string donator = d.TrimEnd('\r', '\n');

				while (donator.Contains("\t\t"))
					donator = donator.Replace("\t\t", "\t");
				string[] props = donator.Split('\t');

				yield return new Donator(int.Parse(props[0]), props[1], int.Parse(props[2]), char.Parse(props[3]));
			}
		}

		public static IEnumerable<Flag> GetFlags(ICommonObjects commonObjects)
		{
			foreach (string f in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "flags")).Split('\n'))
			{
				if (f.StartsWith("?"))
					continue;

				string flag = f.TrimEnd('\r', '\n');

				while (flag.Contains("\t\t"))
					flag = flag.Replace("\t\t", "\t");
				string[] props = flag.Split('\t');

				yield return new Flag(int.Parse(props[0]), props[1]);
			}
		}
	}
}