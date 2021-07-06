using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Users
{
	public class GetUser
	{
		public DateTimeOffset? LockoutEnd { get; init; }

		public bool TwoFactorEnabled { get; init; }

		public bool PhoneNumberConfirmed { get; init; }

		public string? PhoneNumber { get; init; }

		public string? ConcurrencyStamp { get; init; }

		public string? SecurityStamp { get; init; }

		public string? PasswordHash { get; init; }

		public bool EmailConfirmed { get; init; }

		public string? NormalizedEmail { get; init; }

		public string? Email { get; init; }

		public string? NormalizedUserName { get; init; }

		public string? UserName { get; init; }

		public string? Id { get; init; }

		public bool LockoutEnabled { get; init; }

		public int AccessFailedCount { get; init; }

		public List<string> Roles { get; init; } = null!;
	}
}
