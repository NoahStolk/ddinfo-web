using System.Runtime.Serialization;

namespace DevilDaggersWebsite.Enumerators
{
	public enum CustomLeaderboardsClient
	{
		[EnumMember(Value = "DevilDaggersCustomLeaderboards")]
		DevilDaggersCustomLeaderboards = 0,

		[EnumMember(Value = "ddstats-rust")]
		DdstatsRust = 1,
	}
}
