namespace DevilDaggersInfo.Web.Server.Clients.Leaderboard;

public class EntryResponse
{
	public int Rank { get; set; }

	public int Id { get; set; }

	public string Username { get; set; } = null!;

	public int Time { get; set; }

	public int Kills { get; set; }

	public int Gems { get; set; }

	public int DeathType { get; set; }

	public int DaggersHit { get; set; }

	public int DaggersFired { get; set; }

	public ulong TimeTotal { get; set; }

	public ulong KillsTotal { get; set; }

	public ulong GemsTotal { get; set; }

	public ulong DeathsTotal { get; set; }

	public ulong DaggersHitTotal { get; set; }

	public ulong DaggersFiredTotal { get; set; }
}
