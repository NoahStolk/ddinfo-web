using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.WorldRecords
{
	public record GetWorldRecordDataPublic(TimeSpan WorldRecordDuration, double? WorldRecordImprovement);
}
