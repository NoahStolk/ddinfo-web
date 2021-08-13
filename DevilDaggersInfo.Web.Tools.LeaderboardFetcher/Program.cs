using DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardStatistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

byte[] init = await ExecuteRequest(1);

int totalPages = BitConverter.ToInt32(init, 75) / 100 + 1;

string start = DateTime.UtcNow.ToString("yyyyMMddHHmm");
Stopwatch stopwatch = new();
List<CompressedEntry> entries = new();
for (int i = 0; i < totalPages; i++)
{
	stopwatch.Restart();
	entries.AddRange(await Fetch(i * 100 + 1));
	Console.WriteLine($"Fetching page {i + 1}/{totalPages} took {stopwatch.ElapsedMilliseconds / 1000f} seconds.");

	// Write the file after every fetch in case it crashes and all progress is lost.
	File.WriteAllBytes($"{start}.bin", GetBytes(entries));
}

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

static async Task<List<CompressedEntry>> Fetch(int rank)
{
	byte[] data = await ExecuteRequest(rank);

	int entryCount = BitConverter.ToInt16(data, 59);
	int rankIterator = 0;
	int bytePos = 83;

	List<CompressedEntry> compressedEntries = new();
	while (rankIterator < entryCount)
	{
		short usernameLength = BitConverter.ToInt16(data, bytePos);
		bytePos += 2 + usernameLength;

		CompressedEntry entry = new()
		{
			Time = (uint)BitConverter.ToInt32(data, bytePos + 8),
			Kills = (ushort)BitConverter.ToInt32(data, bytePos + 12),
			Gems = (ushort)BitConverter.ToInt32(data, bytePos + 24),
			DaggersHit = (ushort)BitConverter.ToInt32(data, bytePos + 20),
			DaggersFired = (uint)BitConverter.ToInt32(data, bytePos + 16),
			DeathType = (byte)BitConverter.ToInt16(data, bytePos + 28),
		};
		compressedEntries.Add(entry);

		bytePos += 84;
		rankIterator++;
	}

	Console.WriteLine($"Fetched rank {rank} - {rank + 99}.");

	return compressedEntries;
}

static async Task<byte[]> ExecuteRequest(int rank)
{
	List<KeyValuePair<string?, string?>> postValues = new()
	{
		new("user", "0"),
		new("level", "survival"),
		new("offset", (rank - 1).ToString()),
	};

	using FormUrlEncodedContent content = new(postValues);
	using HttpClient client = new();
	HttpResponseMessage response = await client.PostAsync("http://dd.hasmodai.com/backend15/get_scores.php", content);
	return await response.Content.ReadAsByteArrayAsync();
}
