using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.PlayerHistory
{
	public class GetPlayerActivity
	{
		public DateTime DateTime { get; init; }

		public ulong DeathsTotal { get; init; }
	}
}
