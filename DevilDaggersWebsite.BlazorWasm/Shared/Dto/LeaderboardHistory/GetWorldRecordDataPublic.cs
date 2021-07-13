using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistory
{
	public record GetWorldRecordDataPublic(TimeSpan WorldRecordDuration, double? WorldRecordImprovement);
}
