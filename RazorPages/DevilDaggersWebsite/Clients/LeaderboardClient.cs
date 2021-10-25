using DevilDaggersWebsite.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Clients
{
	public sealed class LeaderboardClient
	{
		private const string _getScoresUrl = "http://dd.hasmodai.com/backend15/get_scores.php";
		private const string _getUserSearchUrl = "http://dd.hasmodai.com/backend16/get_user_search_public.php";
		private const string _getUsersByIdsUrl = "http://l.sorath.com/dd/get_multiple_users_by_id_public.php";
		private const string _getUserByIdUrl = "http://dd.hasmodai.com/backend16/get_user_by_id_public.php";

		private readonly HttpClient _httpClient;

		private static readonly Lazy<LeaderboardClient> _lazy = new(() => new());

		private LeaderboardClient()
		{
			_httpClient = new();
		}

		public static LeaderboardClient Instance => _lazy.Value;

		private async Task<byte[]> ExecuteRequest(string url, params KeyValuePair<string?, string?>[] parameters)
		{
			using FormUrlEncodedContent content = new(parameters);
			using HttpResponseMessage response = await _httpClient.PostAsync(url, content);
			return await response.Content.ReadAsByteArrayAsync();
		}

		public async Task<Leaderboard?> GetScores(int rankStart)
		{
			try
			{
				string offset = (rankStart - 1).ToString();
				byte[] data = await ExecuteRequest(_getScoresUrl, new KeyValuePair<string?, string?>("offset", offset));

				Leaderboard leaderboard = new()
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
				int bytePosition = 83;
				while (rankIterator < entryCount)
				{
					leaderboard.Entries.Add(new()
					{
						Username = GetUsername(data, ref bytePosition),
						Rank = BitConverter.ToInt32(data, bytePosition),
						Id = BitConverter.ToInt32(data, bytePosition + 4),
						Time = BitConverter.ToInt32(data, bytePosition + 8),
						Kills = BitConverter.ToInt32(data, bytePosition + 12),
						Gems = BitConverter.ToInt32(data, bytePosition + 24),
						DaggersHit = BitConverter.ToInt32(data, bytePosition + 20),
						DaggersFired = BitConverter.ToInt32(data, bytePosition + 16),
						DeathType = BitConverter.ToInt16(data, bytePosition + 28),
						TimeTotal = BitConverter.ToUInt64(data, bytePosition + 56),
						KillsTotal = BitConverter.ToUInt64(data, bytePosition + 40),
						GemsTotal = BitConverter.ToUInt64(data, bytePosition + 64),
						DeathsTotal = BitConverter.ToUInt64(data, bytePosition + 32),
						DaggersHitTotal = BitConverter.ToUInt64(data, bytePosition + 72),
						DaggersFiredTotal = BitConverter.ToUInt64(data, bytePosition + 48),
					});

					bytePosition += 84;
					rankIterator++;
				}

				return leaderboard;
			}
			catch
			{
				return null;
			}
		}

		public async Task<List<Entry>?> GetUserSearch(string search)
		{
			try
			{
				byte[] data = await ExecuteRequest(_getUserSearchUrl, new KeyValuePair<string?, string?>("search", search));

				int entryCount = BitConverter.ToInt16(data, 11);
				int rankIterator = 0;
				int bytePosition = 19;

				List<Entry> entries = new();
				while (rankIterator < entryCount)
				{
					entries.Add(new()
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
					});

					bytePosition += 88;
					rankIterator++;
				}

				return entries;
			}
			catch
			{
				return null;
			}
		}

		public async Task<List<Entry>?> GetUsersByIds(IEnumerable<int> ids)
		{
			try
			{
				byte[] data = await ExecuteRequest(_getUsersByIdsUrl, new KeyValuePair<string?, string?>("uid", string.Join(',', ids)));

				int bytePosition = 19;
				List<Entry> entries = new();
				while (bytePosition < data.Length)
				{
					entries.Add(new()
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
					});

					bytePosition += 88;
				}

				return entries;
			}
			catch
			{
				return null;
			}
		}

		public async Task<Entry?> GetUserById(int userId)
		{
			try
			{
				byte[] data = await ExecuteRequest(_getUserByIdUrl, new KeyValuePair<string?, string?>("uid", userId.ToString()));

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

		private static string GetUsername(byte[] data, ref int bytePos)
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
