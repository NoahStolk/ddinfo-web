using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;

public enum CustomLeaderboardsClient
{
	[EnumMember(Value = "DevilDaggersCustomLeaderboards")]
	DevilDaggersCustomLeaderboards = 0,

	[EnumMember(Value = "ddstats-rust")]
	DdstatsRust = 1,
}
