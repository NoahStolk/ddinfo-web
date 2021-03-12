using DevilDaggersWebsite.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Clients
{
	public static class DdHasmodaiClient
	{
		private const string _getScoresUrl = "http://dd.hasmodai.com/backend15/get_scores.php";
		private const string _getUserSearchUrl = "http://dd.hasmodai.com/backend16/get_user_search_public.php";
		public static readonly string GetUserByIdUrl = "http://dd.hasmodai.com/backend16/get_user_by_id_public.php";

		public static async Task<Leaderboard?> GetScores(int rank)
		{
			try
			{
				List<KeyValuePair<string?, string?>> postValues = new()
				{
					new("user", "0"),
					new("level", "survival"),
					new("offset", (rank - 1).ToString()),
				};

				using FormUrlEncodedContent content = new(postValues);
				using HttpClient client = new();
				HttpResponseMessage resp = await client.PostAsync(_getScoresUrl, content);
				byte[] data = await resp.Content.ReadAsByteArrayAsync();

				Leaderboard leaderboard = new Leaderboard
				{
					DeathsGlobal = BitConverter.ToUInt64(data, 11),
					KillsGlobal = BitConverter.ToUInt64(data, 19),
					TimeGlobal = BitConverter.ToUInt64(data, 35),
					GemsGlobal = BitConverter.ToUInt64(data, 43),
					Players = BitConverter.ToInt32(data, 75),
					DaggersHitGlobal = BitConverter.ToUInt64(data, 51),
					DaggersFiredGlobal = BitConverter.ToUInt64(data, 27),
				};

				int entryCount = BitConverter.ToInt16(data, 59);
				int rankIterator = 0;
				int bytePos = 83;
				while (rankIterator < entryCount)
				{
					Entry entry = new()
					{
						Username = GetUsername(data, ref bytePos),
						Rank = BitConverter.ToInt32(data, bytePos),
						Id = BitConverter.ToInt32(data, bytePos + 4),
						Time = BitConverter.ToInt32(data, bytePos + 8),
						Kills = BitConverter.ToInt32(data, bytePos + 12),
						Gems = BitConverter.ToInt32(data, bytePos + 24),
						DaggersHit = BitConverter.ToInt32(data, bytePos + 20),
						DaggersFired = BitConverter.ToInt32(data, bytePos + 16),
						DeathType = BitConverter.ToInt16(data, bytePos + 28),
						TimeTotal = BitConverter.ToUInt64(data, bytePos + 56),
						KillsTotal = BitConverter.ToUInt64(data, bytePos + 40),
						GemsTotal = BitConverter.ToUInt64(data, bytePos + 64),
						DeathsTotal = BitConverter.ToUInt64(data, bytePos + 32),
						DaggersHitTotal = BitConverter.ToUInt64(data, bytePos + 72),
						DaggersFiredTotal = BitConverter.ToUInt64(data, bytePos + 48),
					};

					bytePos += 84;

					leaderboard.Entries.Add(entry);

					rankIterator++;
				}

				return leaderboard;
			}
			catch
			{
				return null;
			}
		}

		public static async Task<Leaderboard?> GetUserSearch(string search)
		{
			try
			{
				List<KeyValuePair<string?, string?>> postValues = new()
				{
					new("search", search),
				};

				using FormUrlEncodedContent content = new(postValues);
				using HttpClient client = new();
				HttpResponseMessage resp = await client.PostAsync(_getUserSearchUrl, content);
				byte[] data = await resp.Content.ReadAsByteArrayAsync();

				Leaderboard leaderboard = new();

				int entryCount = BitConverter.ToInt16(data, 11);
				int rankIterator = 0;
				int bytePos = 19;
				while (rankIterator < entryCount)
				{
					Entry entry = new()
					{
						Username = GetUsername(data, ref bytePos),
						Rank = BitConverter.ToInt32(data, bytePos),
						Id = BitConverter.ToInt32(data, bytePos + 4),
						Time = BitConverter.ToInt32(data, bytePos + 12),
						Kills = BitConverter.ToInt32(data, bytePos + 16),
						Gems = BitConverter.ToInt32(data, bytePos + 28),
						DaggersHit = BitConverter.ToInt32(data, bytePos + 24),
						DaggersFired = BitConverter.ToInt32(data, bytePos + 20),
						DeathType = BitConverter.ToInt16(data, bytePos + 32),
						TimeTotal = BitConverter.ToUInt64(data, bytePos + 60),
						KillsTotal = BitConverter.ToUInt64(data, bytePos + 44),
						GemsTotal = BitConverter.ToUInt64(data, bytePos + 68),
						DeathsTotal = BitConverter.ToUInt64(data, bytePos + 36),
						DaggersHitTotal = BitConverter.ToUInt64(data, bytePos + 76),
						DaggersFiredTotal = BitConverter.ToUInt64(data, bytePos + 52),
					};

					bytePos += 88;

					leaderboard.Entries.Add(entry);

					rankIterator++;
				}

				return leaderboard;
			}
			catch
			{
				return null;
			}
		}

		public static async Task<Entry?> GetUserById(int userId)
		{
			try
			{
				List<KeyValuePair<string?, string?>> postValues = new()
				{
					new("uid", userId.ToString()),
				};

				using FormUrlEncodedContent content = new(postValues);
				using HttpClient client = new();
				HttpResponseMessage response = await client.PostAsync(GetUserByIdUrl, content);
				byte[] data = await response.Content.ReadAsByteArrayAsync();

				int bytePosition = 19;

				return new Entry
				{
					Username = GetUsername(data, ref bytePosition),
					Rank = BitConverter.ToInt32(data, bytePosition),
					Id = BitConverter.ToInt32(data, bytePosition + 4),
					Time = BitConverter.ToInt32(data, bytePosition + 12),
					Kills = BitConverter.ToInt32(data, bytePosition + 16),
					Gems = BitConverter.ToInt32(data, bytePosition + 28),
					DaggersHit = BitConverter.ToInt32(data, bytePosition + 24),
					DaggersFired = BitConverter.ToInt32(data, bytePosition + 20),
					DeathType = BitConverter.ToInt16(data, bytePosition + 32),
					TimeTotal = BitConverter.ToUInt64(data, bytePosition + 60),
					KillsTotal = BitConverter.ToUInt64(data, bytePosition + 44),
					GemsTotal = BitConverter.ToUInt64(data, bytePosition + 68),
					DeathsTotal = BitConverter.ToUInt64(data, bytePosition + 36),
					DaggersHitTotal = BitConverter.ToUInt64(data, bytePosition + 76),
					DaggersFiredTotal = BitConverter.ToUInt64(data, bytePosition + 52),
				};
			}
			catch
			{
				return null;
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
