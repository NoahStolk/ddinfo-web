using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using Newtonsoft.Json;
using System.Net;

namespace DevilDaggersWebsite.Code.Utils.Web
{
	public static class Ddstats
	{
		private static readonly string getScoresUrl = "http://ddstats.com/api/get_scores";

		public static Leaderboard LoadLeaderboard(int rank)
		{
			string leaderboardDataJson = GetLeaderboardData(rank);

			return ParseLeaderboardData(leaderboardDataJson);
		}

		private static string GetLeaderboardData(int rank)
		{
			using (WebClient wc = new WebClient())
			{
				return wc.DownloadString($"{getScoresUrl}?offset={rank - 1}");
			}
		}

		private static Leaderboard ParseLeaderboardData(string leaderboardDataJson)
		{
			Leaderboard leaderboard = new Leaderboard();

			dynamic jsonLeaderboard = JsonConvert.DeserializeObject(leaderboardDataJson);

			leaderboard.Players = jsonLeaderboard.global_player_count;
			leaderboard.TimeGlobal = jsonLeaderboard.global_time * 10000;
			leaderboard.GemsGlobal = jsonLeaderboard.global_gems;
			leaderboard.ShotsHitGlobal = jsonLeaderboard.global_daggers_hit;
			leaderboard.ShotsFiredGlobal = jsonLeaderboard.global_daggers_fired;
			leaderboard.KillsGlobal = jsonLeaderboard.global_enemies_killed;
			leaderboard.DeathsGlobal = jsonLeaderboard.global_deaths;

			foreach (dynamic jsonEntry in jsonLeaderboard.entry_list)
			{
				Entry entry = new Entry
				{
					Username = jsonEntry.player_name,
					Id = jsonEntry.player_id,
					Rank = jsonEntry.rank,
					Time = jsonEntry.game_time * 10000,
					Gems = jsonEntry.gems,
					ShotsHit = jsonEntry.daggers_hit,
					ShotsFired = jsonEntry.daggers_fired,
					Kills = jsonEntry.enemies_killed,
					DeathType = GameInfo.GetDeathFromDeathName((string)jsonEntry.death_type).DeathType,
					TimeTotal = jsonEntry.total_game_time * 10000,
					GemsTotal = jsonEntry.total_gems,
					ShotsHitTotal = jsonEntry.total_daggers_hit,
					ShotsFiredTotal = jsonEntry.total_daggers_fired,
					KillsTotal = jsonEntry.total_enemies_killed,
					DeathsTotal = jsonEntry.total_deaths
				};

				leaderboard.Entries.Add(entry);
			}

			return leaderboard;
		}
	}
}