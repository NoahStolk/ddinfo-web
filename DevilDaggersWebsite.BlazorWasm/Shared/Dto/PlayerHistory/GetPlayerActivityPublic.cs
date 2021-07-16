using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.PlayerHistory
{
	public class GetPlayerActivityPublic
	{
		public DateTime DateTime { get; init; }

		public ulong DeathsTotal { get; init; }
	}
}
