using DevilDaggersWebsite.Models.Leaderboard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Utils
{
	public static class LeaderboardUtils
	{
		private static readonly string serverURL = "http://dd.hasmodai.com/backend15/get_scores.php";

		public static async Task<Leaderboard> LoadLeaderboard(int rank)
		{
			byte[] leaderboardData = await GetLeaderboardData(rank);
			return ParseLeaderboardData(leaderboardData);
		}

		private static async Task<byte[]> GetLeaderboardData(int rank)
		{
			Dictionary<string, string> postValues = new Dictionary<string, string>
			{
				{ "user", "0" },
				{ "level", "survival" },
				{ "offset", (rank-1).ToString() }
			};

			FormUrlEncodedContent content = new FormUrlEncodedContent(postValues);
			HttpClient client = new HttpClient();
			HttpResponseMessage resp = await client.PostAsync(serverURL, content);
			return await resp.Content.ReadAsByteArrayAsync();
		}

		private static Leaderboard ParseLeaderboardData(byte[] leaderboardData)
		{
			Leaderboard leaderboard = new Leaderboard
			{
				DeathsGlobal = BitConverter.ToUInt64(leaderboardData, 11),
				KillsGlobal = BitConverter.ToUInt64(leaderboardData, 19),
				TimeGlobal = BitConverter.ToUInt64(leaderboardData, 35),
				GemsGlobal = BitConverter.ToUInt64(leaderboardData, 43),
				Players = BitConverter.ToInt32(leaderboardData, 75),
				ShotsHitGlobal = BitConverter.ToUInt64(leaderboardData, 51),
				ShotsFiredGlobal = BitConverter.ToUInt64(leaderboardData, 27)
			};

			int entryCount = BitConverter.ToInt16(leaderboardData, 59);
			int rankIterator = 0;
			int bytePos = 83;
			while (rankIterator < entryCount)
			{
				Entry entry = new Entry();

				short usernameLength = BitConverter.ToInt16(leaderboardData, bytePos);
				bytePos += 2;

				byte[] usernameBytes = new byte[usernameLength];
				for (int i = bytePos; i < (bytePos + usernameLength); i++)
					usernameBytes[i - bytePos] = leaderboardData[i];
				entry.Username = Encoding.UTF8.GetString(usernameBytes);
				bytePos += usernameLength;

				entry.Rank = BitConverter.ToInt32(leaderboardData, bytePos);
				entry.ID = BitConverter.ToInt32(leaderboardData, bytePos + 4);
				entry.Time = BitConverter.ToInt32(leaderboardData, bytePos + 8);
				entry.Kills = BitConverter.ToInt32(leaderboardData, bytePos + 12);
				entry.Gems = BitConverter.ToInt32(leaderboardData, bytePos + 24);
				entry.ShotsHit = BitConverter.ToInt32(leaderboardData, bytePos + 20);
				entry.ShotsFired = BitConverter.ToInt32(leaderboardData, bytePos + 16);
				entry.DeathType = BitConverter.ToInt16(leaderboardData, bytePos + 28);
				entry.TimeTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 56);
				entry.KillsTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 40);
				entry.GemsTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 64);
				entry.DeathsTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 32);
				entry.ShotsHitTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 72);
				entry.ShotsFiredTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 48);

				bytePos += 84;

				leaderboard.Entries.Add(entry);

				rankIterator++;
			}

			return leaderboard;
		}

		private static readonly string serverSearchURL = "http://dd.hasmodai.com/backend16/get_user_search_public.php";

		public static async Task<Leaderboard> LoadLeaderboardSearch(string search)
		{
			byte[] leaderboardData = await GetLeaderboardSearchData(search);

			return ParseLeaderboardSearchData(leaderboardData);
		}

		private static async Task<byte[]> GetLeaderboardSearchData(string search)
		{
			Dictionary<string, string> postValues = new Dictionary<string, string>
			{
				{ "search", search }
			};

			FormUrlEncodedContent content = new FormUrlEncodedContent(postValues);
			HttpClient client = new HttpClient();
			HttpResponseMessage resp = await client.PostAsync(serverSearchURL, content);
			return await resp.Content.ReadAsByteArrayAsync();
		}

		private static Leaderboard ParseLeaderboardSearchData(byte[] leaderboardData)
		{
			Leaderboard leaderboard = new Leaderboard();

			int entryCount = BitConverter.ToInt16(leaderboardData, 11);
			int rankIterator = 0;
			int bytePos = 19;
			while (rankIterator < entryCount)
			{
				Entry entry = new Entry();

				short usernameLength = BitConverter.ToInt16(leaderboardData, bytePos);
				bytePos += 2;

				byte[] usernameBytes = new byte[usernameLength];
				for (int i = bytePos; i < (bytePos + usernameLength); i++)
					usernameBytes[i - bytePos] = leaderboardData[i];
				entry.Username = Encoding.UTF8.GetString(usernameBytes);
				bytePos += usernameLength;

				entry.Rank = BitConverter.ToInt32(leaderboardData, bytePos);
				entry.ID = BitConverter.ToInt32(leaderboardData, bytePos + 4);
				entry.Time = BitConverter.ToInt32(leaderboardData, bytePos + 12);
				entry.Kills = BitConverter.ToInt32(leaderboardData, bytePos + 16);
				entry.Gems = BitConverter.ToInt32(leaderboardData, bytePos + 28);
				entry.ShotsHit = BitConverter.ToInt32(leaderboardData, bytePos + 24);
				entry.ShotsFired = BitConverter.ToInt32(leaderboardData, bytePos + 20);
				entry.DeathType = BitConverter.ToInt16(leaderboardData, bytePos + 32);
				entry.TimeTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 60);
				entry.KillsTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 44);
				entry.GemsTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 68);
				entry.DeathsTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 36);
				entry.ShotsHitTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 76);
				entry.ShotsFiredTotal = BitConverter.ToUInt64(leaderboardData, bytePos + 52);

				bytePos += 88;

				leaderboard.Entries.Add(entry);

				rankIterator++;
			}

			return leaderboard;
		}

		private static readonly string serverURLJson = "http://ddstats.com/api/get_scores";

		public static Leaderboard LoadLeaderboardJson(int rank)
		{
			string leaderboardDataJson = GetLeaderboardDataJson(rank);

			return ParseLeaderboardDataJson(leaderboardDataJson);
		}

		private static string GetLeaderboardDataJson(int rank)
		{
			using (WebClient wc = new WebClient())
			{
				return wc.DownloadString($"{serverURLJson}?offset={rank - 1}");
			}
		}

		private static Leaderboard ParseLeaderboardDataJson(string leaderboardDataJson)
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
					ID = jsonEntry.player_id,
					Rank = jsonEntry.rank,
					Time = jsonEntry.game_time * 10000,
					Gems = jsonEntry.gems,
					ShotsHit = jsonEntry.daggers_hit,
					ShotsFired = jsonEntry.daggers_fired,
					Kills = jsonEntry.enemies_killed,
					DeathType = GameUtils.GetDeathTypeFromDeathName((string)jsonEntry.death_type),
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