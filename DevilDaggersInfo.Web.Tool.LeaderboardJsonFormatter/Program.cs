using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using Newtonsoft.Json;

DateTime fullHistoryDateStart = new(2018, 10, 1);

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S1075 // URIs should not be hardcoded
foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersInfo\DevilDaggersInfo.Web.BlazorWasm\Server\Data\LeaderboardHistory", "*.json"))
#pragma warning restore S1075 // URIs should not be hardcoded
#pragma warning restore IDE0079 // Remove unnecessary suppression
{
	try
	{
		GetLeaderboardHistory leaderboard = JsonConvert.DeserializeObject<GetLeaderboardHistory>(File.ReadAllText(path)) ?? throw new("Could not deserialize leaderboard.");
		Formatting formatting = leaderboard.DateTime > fullHistoryDateStart ? Formatting.None : Formatting.Indented;
		File.WriteAllText(path, JsonConvert.SerializeObject(leaderboard, formatting));

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"SUCCESS for {path}: {formatting}");
	}
	catch (Exception ex)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"FAIL for {path}: {ex.Message}");
	}
}
