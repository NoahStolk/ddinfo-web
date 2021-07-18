using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Users
{
	public class GetUser : IGetDto<string>
	{
		public string Id { get; init; } = null!;

		public string? UserName { get; init; }

		public List<string> Roles { get; init; } = null!;
	}
}
