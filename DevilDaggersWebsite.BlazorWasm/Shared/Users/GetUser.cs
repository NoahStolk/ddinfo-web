using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Users
{
	public class GetUser
	{
		public string? Id { get; init; }

		public string? UserName { get; init; }

		public List<string> Roles { get; init; } = null!;
	}
}
