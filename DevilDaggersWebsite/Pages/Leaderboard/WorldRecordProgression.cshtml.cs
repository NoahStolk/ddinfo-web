using CoreBase.Services;
using DevilDaggersWebsite.Models.Leaderboard;
using DevilDaggersWebsite.Pages.API;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class WorldRecordProgressionModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public Dictionary<string, TimeSpan> TopUsers { get; set; } = new Dictionary<string, TimeSpan>();

		public WorldRecordProgressionModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet()
		{
			SortedDictionary<DateTime, Entry> data = new GetWorldRecordsModel(_commonObjects).GetWorldRecords();

			List<Tuple<int, DateTime, Entry>> data1 = new List<Tuple<int, DateTime, Entry>>();

			int i = 0;
			foreach (KeyValuePair<DateTime, Entry> kvp in data)
			{
				if (kvp.Key < GameUtils.V1.ReleaseDate)
					continue;

				data1.Add(Tuple.Create(i, kvp.Key, kvp.Value));
				i++;
			}

			i = 0;
			foreach (Tuple<int, DateTime, Entry> tuple in data1)
			{
				TimeSpan diff;
				if (i == data1.Count - 1)
					diff = DateTime.Now - tuple.Item2;
				else
					diff = data1.Where(t => t.Item1 == i + 1).FirstOrDefault().Item2 - tuple.Item2;
				i++;

				if (!TopUsers.ContainsKey(tuple.Item3.Username))
					TopUsers[tuple.Item3.Username] = diff;
				else
					TopUsers[tuple.Item3.Username] += diff;
			}

			TopUsers = TopUsers.OrderByDescending(kvp => kvp.Value).ToDictionary(
				mc => mc.Key,
				mc => mc.Value);
		}
	}
}