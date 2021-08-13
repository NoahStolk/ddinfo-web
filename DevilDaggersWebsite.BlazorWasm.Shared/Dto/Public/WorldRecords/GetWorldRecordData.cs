using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.WorldRecords
{
	public record GetWorldRecordData(TimeSpan WorldRecordDuration, double? WorldRecordImprovement);
}
