using DevilDaggersCore.Game;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.WorldRecords
{
	public class GetWorldRecordPublic
	{
		public DateTime DateTime { get; init; }
		public GetEntryHistory Entry { get; init; } = null!;
		public GameVersion? GameVersion { get; init; }
	}
}
