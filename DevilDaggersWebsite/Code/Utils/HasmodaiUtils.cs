using DevilDaggersCore.Leaderboards;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.External
{
	public static class HasmodaiUtils
	{
		private static readonly string getScoresUrl = "http://dd.hasmodai.com/backend15/get_scores.php";
		private static readonly string getUserSearchUrl = "http://dd.hasmodai.com/backend16/get_user_search_public.php";
		public static readonly string GetUserByIdUrl = "http://dd.hasmodai.com/backend16/get_user_by_id_public.php";

		public static async Task<Leaderboard> GetScores(int rank)
		{
			try
			{
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
					Entry entry = new Entry
					{
						Username = GetUsername(data, ref bytePos),
						Rank = BitConverter.ToInt32(data, bytePos),
						Id = BitConverter.ToInt32(data, bytePos + 4),
						Time = BitConverter.ToInt32(data, bytePos + 8),
						Kills = BitConverter.ToInt32(data, bytePos + 12),
						Gems = BitConverter.ToInt32(data, bytePos + 24),
						ShotsHit = BitConverter.ToInt32(data, bytePos + 20),
						ShotsFired = BitConverter.ToInt32(data, bytePos + 16),
						DeathType = BitConverter.ToInt16(data, bytePos + 28),
						TimeTotal = BitConverter.ToUInt64(data, bytePos + 56),
						KillsTotal = BitConverter.ToUInt64(data, bytePos + 40),
						GemsTotal = BitConverter.ToUInt64(data, bytePos + 64),
						DeathsTotal = BitConverter.ToUInt64(data, bytePos + 32),
						ShotsHitTotal = BitConverter.ToUInt64(data, bytePos + 72),
						ShotsFiredTotal = BitConverter.ToUInt64(data, bytePos + 48)
					};

					bytePos += 84;

					leaderboard.Entries.Add(entry);

					rankIterator++;
				}

				return leaderboard;
			}
			catch
			{
				return new Leaderboard();
			}
		}

		public static async Task<Leaderboard> GetUserSearch(string search)
		{
			try
			{
				Dictionary<string, string> postValues = new Dictionary<string, string>
				{
					{ "search", search }
				};

				FormUrlEncodedContent content = new FormUrlEncodedContent(postValues);
				HttpClient client = new HttpClient();
				HttpResponseMessage resp = await client.PostAsync(getUserSearchUrl, content);
				byte[] data = await resp.Content.ReadAsByteArrayAsync();

				Leaderboard leaderboard = new Leaderboard();

				int entryCount = BitConverter.ToInt16(data, 11);
				int rankIterator = 0;
				int bytePos = 19;
				while (rankIterator < entryCount)
				{
					Entry entry = new Entry
					{
						Username = GetUsername(data, ref bytePos),
						Rank = BitConverter.ToInt32(data, bytePos),
						Id = BitConverter.ToInt32(data, bytePos + 4),
						Time = BitConverter.ToInt32(data, bytePos + 12),
						Kills = BitConverter.ToInt32(data, bytePos + 16),
						Gems = BitConverter.ToInt32(data, bytePos + 28),
						ShotsHit = BitConverter.ToInt32(data, bytePos + 24),
						ShotsFired = BitConverter.ToInt32(data, bytePos + 20),
						DeathType = BitConverter.ToInt16(data, bytePos + 32),
						TimeTotal = BitConverter.ToUInt64(data, bytePos + 60),
						KillsTotal = BitConverter.ToUInt64(data, bytePos + 44),
						GemsTotal = BitConverter.ToUInt64(data, bytePos + 68),
						DeathsTotal = BitConverter.ToUInt64(data, bytePos + 36),
						ShotsHitTotal = BitConverter.ToUInt64(data, bytePos + 76),
						ShotsFiredTotal = BitConverter.ToUInt64(data, bytePos + 52)
					};

					bytePos += 88;

					leaderboard.Entries.Add(entry);

					rankIterator++;
				}

				return leaderboard;
			}
			catch
			{
				return new Leaderboard();
			}
		}

		// TODO: Remove.
		public static async Task<Entry> GetUserById(int userId)
		{
			try
			{
				Dictionary<string, string> postValues = new Dictionary<string, string>
				{
					{ "uid", userId.ToString() }
				};

				FormUrlEncodedContent content = new FormUrlEncodedContent(postValues);
				HttpClient client = new HttpClient();
				HttpResponseMessage resp = await client.PostAsync(GetUserByIdUrl, content);
				byte[] data = await resp.Content.ReadAsByteArrayAsync();

				int bytePos = 19;

				Entry entry = new Entry
				{
					Username = GetUsername(data, ref bytePos),
					Rank = BitConverter.ToInt32(data, bytePos),
					Id = BitConverter.ToInt32(data, bytePos + 4),
					Time = BitConverter.ToInt32(data, bytePos + 12),
					Kills = BitConverter.ToInt32(data, bytePos + 16),
					Gems = BitConverter.ToInt32(data, bytePos + 28),
					ShotsHit = BitConverter.ToInt32(data, bytePos + 24),
					ShotsFired = BitConverter.ToInt32(data, bytePos + 20),
					DeathType = BitConverter.ToInt16(data, bytePos + 32),
					TimeTotal = BitConverter.ToUInt64(data, bytePos + 60),
					KillsTotal = BitConverter.ToUInt64(data, bytePos + 44),
					GemsTotal = BitConverter.ToUInt64(data, bytePos + 68),
					DeathsTotal = BitConverter.ToUInt64(data, bytePos + 36),
					ShotsHitTotal = BitConverter.ToUInt64(data, bytePos + 76),
					ShotsFiredTotal = BitConverter.ToUInt64(data, bytePos + 52)
				};

				return entry;
			}
			catch
			{
				return new Entry();
			}
		}

		public static string GetUsername(byte[] data, ref int bytePos)
		{
			short usernameLength = BitConverter.ToInt16(data, bytePos);
			bytePos += 2;

			byte[] usernameBytes = new byte[usernameLength];
			Buffer.BlockCopy(data, bytePos, usernameBytes, 0, usernameLength);

			bytePos += usernameLength;
			return Encoding.UTF8.GetString(usernameBytes);
		}
	}
}