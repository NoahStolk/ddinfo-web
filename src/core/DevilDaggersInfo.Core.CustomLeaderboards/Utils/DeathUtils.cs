namespace DevilDaggersInfo.Core.CustomLeaderboards.Utils;

// TODO: Reference Core.Wiki?
public static class DeathUtils
{
	private static readonly Dictionary<byte, string> _names = new()
	{
		[0] = "FALLEN",
		[1] = "SWARMED",
		[2] = "IMPALED",
		[3] = "GORED",
		[4] = "INFESTED",
		[5] = "OPENED",
		[6] = "PURGED",
		[7] = "DESECRATED",
		[8] = "SACRIFICED",
		[9] = "EVISCERATED",
		[10] = "ANNIHILATED",
		[11] = "INTOXICATED",
		[12] = "ENVENOMATED",
		[13] = "INCARNATED",
		[14] = "DISCARNATED",
		[15] = "ENTANGLED",
		[16] = "HAUNTED",
	};

	public static string GetName(byte deathType)
	{
		if (_names.ContainsKey(deathType))
			return _names[deathType];

		return "Invalid death type";
	}
}
