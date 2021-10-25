namespace DevilDaggersInfo.Web.BlazorWasm.Server.Clients.Leaderboard;

public sealed class LeaderboardClient
{
#pragma warning disable S1075 // URIs should not be hardcoded
	private const string _getScoresUrl = "http://dd.hasmodai.com/backend15/get_scores.php";
	private const string _getUserSearchUrl = "http://dd.hasmodai.com/backend16/get_user_search_public.php";
	private const string _getUsersByIdsUrl = "http://l.sorath.com/dd/get_multiple_users_by_id_public.php";
	private const string _getUserByIdUrl = "http://dd.hasmodai.com/backend16/get_user_by_id_public.php";
#pragma warning restore S1075 // URIs should not be hardcoded

	private readonly HttpClient _httpClient;

	private static readonly Lazy<LeaderboardClient> _lazy = new(() => new());

	private LeaderboardClient()
	{
		_httpClient = new();
	}

	public static LeaderboardClient Instance => _lazy.Value;

	private async Task<MemoryStream> ExecuteRequest(string url, params KeyValuePair<string?, string?>[] parameters)
	{
		using FormUrlEncodedContent content = new(parameters);
		using HttpResponseMessage response = await _httpClient.PostAsync(url, content);

		MemoryStream ms = new();
		await response.Content.CopyToAsync(ms);
		return ms;
	}

	public async Task<LeaderboardResponse> GetLeaderboard(int rankStart)
	{
		using BinaryReader br = new(await ExecuteRequest(_getScoresUrl, new KeyValuePair<string?, string?>("offset", (rankStart - 1).ToString())));

		LeaderboardResponse leaderboard = new()
		{
			DateTime = DateTime.UtcNow,
		};

		br.BaseStream.Seek(11, SeekOrigin.Begin);
		leaderboard.DeathsGlobal = br.ReadUInt64();
		leaderboard.KillsGlobal = br.ReadUInt64();
		leaderboard.DaggersFiredGlobal = br.ReadUInt64();
		leaderboard.TimeGlobal = br.ReadUInt64();
		leaderboard.GemsGlobal = br.ReadUInt64();
		leaderboard.DaggersHitGlobal = br.ReadUInt64();
		leaderboard.TotalEntries = br.ReadUInt16();

		br.BaseStream.Seek(14, SeekOrigin.Current);
		leaderboard.TotalPlayers = br.ReadInt32();

		br.BaseStream.Seek(4, SeekOrigin.Current);
		for (int i = 0; i < leaderboard.TotalEntries; i++)
		{
			EntryResponse entry = new();

			short usernameLength = br.ReadInt16();
			entry.Username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
			entry.Rank = br.ReadInt32();
			entry.Id = br.ReadInt32();
			entry.Time = br.ReadInt32();
			entry.Kills = br.ReadInt32();
			entry.DaggersFired = br.ReadInt32();
			entry.DaggersHit = br.ReadInt32();
			entry.Gems = br.ReadInt32();
			entry.DeathType = br.ReadInt32();
			entry.DeathsTotal = br.ReadUInt64();
			entry.KillsTotal = br.ReadUInt64();
			entry.DaggersFiredTotal = br.ReadUInt64();
			entry.TimeTotal = br.ReadUInt64();
			entry.GemsTotal = br.ReadUInt64();
			entry.DaggersHitTotal = br.ReadUInt64();

			br.BaseStream.Seek(4, SeekOrigin.Current);

			leaderboard.Entries.Add(entry);
		}

		return leaderboard;
	}

	public async Task<List<EntryResponse>> GetEntriesByName(string name)
	{
		using BinaryReader br = new(await ExecuteRequest(_getUserSearchUrl, new KeyValuePair<string?, string?>("search", name)));

		List<EntryResponse> entries = new();

		br.BaseStream.Seek(11, SeekOrigin.Begin);
		short totalResults = br.ReadInt16();

		br.BaseStream.Seek(6, SeekOrigin.Current);
		for (int i = 0; i < totalResults; i++)
		{
			EntryResponse entry = new();

			short usernameLength = br.ReadInt16();
			entry.Username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
			entry.Rank = br.ReadInt32();
			entry.Id = br.ReadInt32();

			br.BaseStream.Seek(4, SeekOrigin.Current);
			entry.Time = br.ReadInt32();
			entry.Kills = br.ReadInt32();
			entry.DaggersFired = br.ReadInt32();
			entry.DaggersHit = br.ReadInt32();
			entry.Gems = br.ReadInt32();
			entry.DeathType = br.ReadInt32();
			entry.DeathsTotal = br.ReadUInt64();
			entry.KillsTotal = br.ReadUInt64();
			entry.DaggersFiredTotal = br.ReadUInt64();
			entry.TimeTotal = br.ReadUInt64();
			entry.GemsTotal = br.ReadUInt64();
			entry.DaggersHitTotal = br.ReadUInt64();

			br.BaseStream.Seek(4, SeekOrigin.Current);

			entries.Add(entry);
		}

		return entries;
	}

	public async Task<List<EntryResponse>> GetEntriesByIds(IEnumerable<int> ids)
	{
		using BinaryReader br = new(await ExecuteRequest(_getUsersByIdsUrl, new KeyValuePair<string?, string?>("uid", string.Join(',', ids))));

		List<EntryResponse> entries = new();

		br.BaseStream.Seek(19, SeekOrigin.Begin);
		int entryCount = ids.Count();
		for (int i = 0; i < entryCount; i++)
		{
			EntryResponse entry = new();

			short usernameLength = br.ReadInt16();
			entry.Username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
			entry.Rank = br.ReadInt32();
			entry.Id = br.ReadInt32();

			br.BaseStream.Seek(4, SeekOrigin.Current);
			entry.Time = br.ReadInt32();
			entry.Kills = br.ReadInt32();
			entry.DaggersFired = br.ReadInt32();
			entry.DaggersHit = br.ReadInt32();
			entry.Gems = br.ReadInt32();
			entry.DeathType = br.ReadInt32();
			entry.DeathsTotal = br.ReadUInt64();
			entry.KillsTotal = br.ReadUInt64();
			entry.DaggersFiredTotal = br.ReadUInt64();
			entry.TimeTotal = br.ReadUInt64();
			entry.GemsTotal = br.ReadUInt64();
			entry.DaggersHitTotal = br.ReadUInt64();

			br.BaseStream.Seek(4, SeekOrigin.Current);

			entries.Add(entry);
		}

		return entries;
	}

	public async Task<EntryResponse> GetEntryById(int id)
	{
		using BinaryReader br = new(await ExecuteRequest(_getUserByIdUrl, new KeyValuePair<string?, string?>("uid", id.ToString())));

		EntryResponse entry = new();

		br.BaseStream.Seek(19, SeekOrigin.Begin);

		short usernameLength = br.ReadInt16();
		entry.Username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));
		entry.Rank = br.ReadInt32();
		entry.Id = br.ReadInt32();

		br.BaseStream.Seek(4, SeekOrigin.Current);
		entry.Time = br.ReadInt32();
		entry.Kills = br.ReadInt32();
		entry.DaggersFired = br.ReadInt32();
		entry.DaggersHit = br.ReadInt32();
		entry.Gems = br.ReadInt32();
		entry.DeathType = br.ReadInt32();
		entry.DeathsTotal = br.ReadUInt64();
		entry.KillsTotal = br.ReadUInt64();
		entry.DaggersFiredTotal = br.ReadUInt64();
		entry.TimeTotal = br.ReadUInt64();
		entry.GemsTotal = br.ReadUInt64();
		entry.DaggersHitTotal = br.ReadUInt64();

		return entry;
	}
}
