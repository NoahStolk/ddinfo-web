using DevilDaggersWebsite.Helpers;
using DevilDaggersWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages
{
	public class LeaderboardModel : PageModel
	{
		public int Offset { get; set; } = 1;
		public int OffsetPrevious { get; set; } = 1;

		public List<Entry> Entries { get; set; } = new List<Entry>();

		public int Players { get; set; }
		public UInt64 TimeGlobal { get; set; }
		public UInt64 KillsGlobal { get; set; }
		public UInt64 GemsGlobal { get; set; }
		public UInt64 DeathsGlobal { get; set; }
		public UInt64 ShotsHitGlobal { get; set; }
		public UInt64 ShotsFiredGlobal { get; set; }

		public async Task OnGetAsync()
		{
			await LeaderboardParser.LoadLeaderboard(this);
		}

		public async Task OnPostAsync(string submitAction)
		{
			//ModelState.Remove("OffsetPrevious");

			switch (submitAction)
			{
				case ">":
					Offset = OffsetPrevious + 100;
					break;
				case "<":
					Offset = OffsetPrevious - 100;
					break;
			}

			Offset = Math.Max(1, Offset);
			await LeaderboardParser.LoadLeaderboard(this);

			if (Offset > Players - 99)
			{
				Offset = Players - 99;
				Entries.Clear();
				await LeaderboardParser.LoadLeaderboard(this);
			}
			OffsetPrevious = Offset;
		}
	}
}