using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Tools
{
	public class GetChangelogEntryPublic
	{
		public Version VersionNumber { get; init; } = null!;

		public DateTime Date { get; init; }

		public IReadOnlyList<GetChangePublic> Changes { get; init; } = new List<GetChangePublic>();
	}
}
