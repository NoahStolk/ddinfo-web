namespace DevilDaggersInfo.Web.BlazorWasm.Shared;

public static class Roles
{
	public const string Admin = nameof(Admin);
	public const string CustomLeaderboards = nameof(CustomLeaderboards);
	public const string Donations = nameof(Donations);
	public const string Mods = nameof(Mods);
	public const string Players = nameof(Players);
	public const string Spawnsets = nameof(Spawnsets);

	public static string[] All { get; } = new[]
	{
		Admin,
		CustomLeaderboards,
		Donations,
		Mods,
		Players,
		Spawnsets,
	};
}
