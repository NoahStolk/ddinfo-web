using System;

namespace DevilDaggersWebsite.BlazorWasm.Server.WorldRecords
{
	public record WorldRecordData(TimeSpan WorldRecordDuration, int? WorldRecordImprovement);
}
