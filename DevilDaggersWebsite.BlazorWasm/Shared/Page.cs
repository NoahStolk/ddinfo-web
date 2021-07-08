using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared
{
	public class Page<T>
	{
		public List<T> Results { get; init; } = null!;
		public int TotalResults { get; init; }
	}
}
