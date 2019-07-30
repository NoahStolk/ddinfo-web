using DevilDaggersCore.Leaderboards;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Utils.Web
{
	public static class Hasmodai
	{
		private static readonly string getScoresUrl = "http://dd.hasmodai.com/backend15/get_scores.php";
		private static readonly string getUserSearchUrl = "http://dd.hasmodai.com/backend16/get_user_search_public.php";
		private static readonly string getUserByID = "http://dd.hasmodai.com/backend16/get_user_by_id_public.php";

		public static async Task<Leaderboard> GetScores(int rank)
		{
			// Request the data from the server
			Dictionary<string, string> postValues = new Dictionary<string, string>
			{
				{ "user", "0" },
				{ "level", "survival" },
				{ "offset", (rank-1).ToString() }
			};

			FormUrlEncodedContent content = new FormUrlEncodedContent(postValues);
			HttpClient client = new HttpClient();
			HttpResponseMessage resp = await client.PostAsync(getScoresUrl, content);
			byte[] data = await resp.Content.ReadAsByteArrayAsync();

			// Parse the data
			Leaderboard leaderboard = new Leaderboard
			{
				DeathsGlobal = BitConverter.ToUInt64(data, 11),
				KillsGlobal = BitConverter.ToUInt64(data, 19),
				TimeGlobal = BitConverter.ToUInt64(data, 35),
				GemsGlobal = BitConverter.ToUInt64(data, 43),
				Players = BitConverter.ToInt32(data, 75),
				ShotsHitGlobal = BitConverter.ToUInt64(data, 51),
				ShotsFiredGlobal = BitConverter.ToUInt64(data, 27)
			};

			int entryCount = BitConverter.ToInt16(data, 59);
			int rankIterator = 0;
			int bytePos = 83;
			while (rankIterator < entryCount)
			{
				Entry entry = new Entry();

				bytePos = GetUsername(data, bytePos, entry);

				entry.Rank = BitConverter.ToInt32(data, bytePos);
				entry.ID = BitConverter.ToInt32(data, bytePos + 4);
				entry.Time = BitConverter.ToInt32(data, bytePos + 8);
				entry.Kills = BitConverter.ToInt32(data, bytePos + 12);
				entry.Gems = BitConverter.ToInt32(data, bytePos + 24);
				entry.ShotsHit = BitConverter.ToInt32(data, bytePos + 20);
				entry.ShotsFired = BitConverter.ToInt32(data, bytePos + 16);
				entry.DeathType = BitConverter.ToInt16(data, bytePos + 28);
				entry.TimeTotal = BitConverter.ToUInt64(data, bytePos + 56);
				entry.KillsTotal = BitConverter.ToUInt64(data, bytePos + 40);
				entry.GemsTotal = BitConverter.ToUInt64(data, bytePos + 64);
				entry.DeathsTotal = BitConverter.ToUInt64(data, bytePos + 32);
				entry.ShotsHitTotal = BitConverter.ToUInt64(data, bytePos + 72);
				entry.ShotsFiredTotal = BitConverter.ToUInt64(data, bytePos + 48);

				bytePos += 84;

				leaderboard.Entries.Add(entry);

				rankIterator++;
			}

			return leaderboard;
		}

		public static async Task<Leaderboard> GetUserSearch(string search)
		{
			// Request the data from the server
			Dictionary<string, string> postValues = new Dictionary<string, string>
			{
				{ "search", search }
			};

			FormUrlEncodedContent content = new FormUrlEncodedContent(postValues);
			HttpClient client = new HttpClient();
			HttpResponseMessage resp = await client.PostAsync(getUserSearchUrl, content);
			byte[] data = await resp.Content.ReadAsByteArrayAsync();

			// Parse the data
			Leaderboard leaderboard = new Leaderboard();

			int entryCount = BitConverter.ToInt16(data, 11);
			int rankIterator = 0;
			int bytePos = 19;
			while (rankIterator < entryCount)
			{
				Entry entry = new Entry();

				bytePos = GetUsername(data, bytePos, entry);

				entry.Rank = BitConverter.ToInt32(data, bytePos);
				entry.ID = BitConverter.ToInt32(data, bytePos + 4);
				entry.Time = BitConverter.ToInt32(data, bytePos + 12);
				entry.Kills = BitConverter.ToInt32(data, bytePos + 16);
				entry.Gems = BitConverter.ToInt32(data, bytePos + 28);
				entry.ShotsHit = BitConverter.ToInt32(data, bytePos + 24);
				entry.ShotsFired = BitConverter.ToInt32(data, bytePos + 20);
				entry.DeathType = BitConverter.ToInt16(data, bytePos + 32);
				entry.TimeTotal = BitConverter.ToUInt64(data, bytePos + 60);
				entry.KillsTotal = BitConverter.ToUInt64(data, bytePos + 44);
				entry.GemsTotal = BitConverter.ToUInt64(data, bytePos + 68);
				entry.DeathsTotal = BitConverter.ToUInt64(data, bytePos + 36);
				entry.ShotsHitTotal = BitConverter.ToUInt64(data, bytePos + 76);
				entry.ShotsFiredTotal = BitConverter.ToUInt64(data, bytePos + 52);

				bytePos += 88;

				leaderboard.Entries.Add(entry);

				rankIterator++;
			}

			return leaderboard;
		}

		public static async Task<Entry> GetUserByID(int id)
		{
			// Request the data from the server
			Dictionary<string, string> postValues = new Dictionary<string, string>
			{
				{ "uid", id.ToString() }
			};

			FormUrlEncodedContent content = new FormUrlEncodedContent(postValues);
			HttpClient client = new HttpClient();
			HttpResponseMessage resp = await client.PostAsync(getUserByID, content);
			byte[] data = await resp.Content.ReadAsByteArrayAsync();

			// Parse the data
			Entry entry = new Entry();

			int bytePos = 19;

			bytePos = GetUsername(data, bytePos, entry);

			entry.Rank = BitConverter.ToInt32(data, bytePos);
			entry.ID = BitConverter.ToInt32(data, bytePos + 4);
			entry.Time = BitConverter.ToInt32(data, bytePos + 12);
			entry.Kills = BitConverter.ToInt32(data, bytePos + 16);
			entry.Gems = BitConverter.ToInt32(data, bytePos + 28);
			entry.ShotsHit = BitConverter.ToInt32(data, bytePos + 24);
			entry.ShotsFired = BitConverter.ToInt32(data, bytePos + 20);
			entry.DeathType = BitConverter.ToInt16(data, bytePos + 32);
			entry.TimeTotal = BitConverter.ToUInt64(data, bytePos + 60);
			entry.KillsTotal = BitConverter.ToUInt64(data, bytePos + 44);
			entry.GemsTotal = BitConverter.ToUInt64(data, bytePos + 68);
			entry.DeathsTotal = BitConverter.ToUInt64(data, bytePos + 36);
			entry.ShotsHitTotal = BitConverter.ToUInt64(data, bytePos + 76);
			entry.ShotsFiredTotal = BitConverter.ToUInt64(data, bytePos + 52);

			return entry;
		}

		// Can I have a reference to an int please?
		private static int GetUsername(byte[] data, int bytePos, Entry entry)
		{
			short usernameLength = BitConverter.ToInt16(data, bytePos);
			bytePos += 2;

			byte[] usernameBytes = new byte[usernameLength];
			for (int i = bytePos; i < (bytePos + usernameLength); i++)
				usernameBytes[i - bytePos] = data[i];
			entry.Username = Encoding.UTF8.GetString(usernameBytes);
			bytePos += usernameLength;
			return bytePos;
		}
	}
}