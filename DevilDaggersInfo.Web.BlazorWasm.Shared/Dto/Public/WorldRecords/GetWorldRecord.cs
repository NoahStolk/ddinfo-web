using DevilDaggersCore.Game;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using System;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.WorldRecords
{
	public class GetWorldRecord
	{
		public DateTime DateTime { get; init; }
		public GetEntryHistory Entry { get; init; } = null!;
		public GameVersion? GameVersion { get; init; }
	}
}
