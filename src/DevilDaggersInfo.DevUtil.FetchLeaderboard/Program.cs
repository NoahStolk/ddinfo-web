using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardStatistics;
using System.Diagnostics;

string start = DateTime.UtcNow.ToString("yyyyMMddHHmm");
long startTimeStamp = Stopwatch.GetTimestamp();
List<CompressedEntry> entries = [];
int offset = 0;
while (true)
{
	long subTimeStamp = Stopwatch.GetTimestamp();

	(int totalPlayers, List<CompressedEntry> compressedEntries) = await FetchAsync(offset);
	if (compressedEntries.Count == 0)
		break;

	entries.AddRange(compressedEntries);
	offset += compressedEntries.Count;

	float percentage = totalPlayers == 0 ? 0 : (float)offset / totalPlayers;
	Console.WriteLine($"Fetched ranks {offset}/{totalPlayers} ({percentage:0.00%}). Processing took {Stopwatch.GetElapsedTime(subTimeStamp).TotalSeconds:0.00} seconds.");
}

Console.WriteLine($"Total processing time: {Stopwatch.GetElapsedTime(startTimeStamp)}");

await File.WriteAllBytesAsync($"{start}.bin", GetBytes(entries));

static byte[] GetBytes(List<CompressedEntry> entries)
{
	using MemoryStream ms = new();
	using BinaryWriter bw = new(ms);
	foreach (CompressedEntry ce in entries)
	{
		bw.Write(ce.Time);
		bw.Write(ce.Kills);
		bw.Write(ce.Gems);
		bw.Write(ce.DaggersHit);
		bw.Write(ce.DaggersFired);
		bw.Write(ce.DeathType);
	}

	return ms.ToArray();
}

static async Task<(int TotalPlayers, List<CompressedEntry> Entries)> FetchAsync(int rank)
{
	byte[] data = await ExecuteRequest(rank);

	int entryCount = BitConverter.ToInt16(data, 59);
	int totalPlayers = BitConverter.ToInt32(data, 75);
	int rankIterator = 0;
	int bytePos = 83;

	List<CompressedEntry> compressedEntries = [];
	while (rankIterator < entryCount)
	{
		short usernameLength = BitConverter.ToInt16(data, bytePos);
		bytePos += 2 + usernameLength;

		CompressedEntry entry = new()
		{
			Time = (uint)BitConverter.ToInt32(data, bytePos + 12),
			Kills = (ushort)BitConverter.ToInt32(data, bytePos + 16),
			Gems = (ushort)BitConverter.ToInt32(data, bytePos + 28),
			DaggersHit = (ushort)BitConverter.ToInt32(data, bytePos + 24),
			DaggersFired = (uint)BitConverter.ToInt32(data, bytePos + 20),
			DeathType = (byte)BitConverter.ToInt16(data, bytePos + 32),
		};
		compressedEntries.Add(entry);

		bytePos += 88;
		rankIterator++;
	}

	return (totalPlayers, compressedEntries);
}

static async Task<byte[]> ExecuteRequest(int offset)
{
	List<KeyValuePair<string?, string?>> postValues =
	[
		new("user", "0"),
		new("level", "survival"),
		new("offset", offset.ToString()),
	];

	using FormUrlEncodedContent content = new(postValues);
	using HttpClient client = new();
	HttpResponseMessage response = await client.PostAsync(new Uri("http://dd.hasmodai.com/dd3/get_scores.php"), content);
	return await response.Content.ReadAsByteArrayAsync();
}
