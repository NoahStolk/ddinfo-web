using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class Tool
	{
		public string Name { get; init; } = null!;

		public string DisplayName { get; set; } = null!;

		/// <summary>
		/// Indicates the current version of the tool on the website.
		/// </summary>
		public Version VersionNumber { get; set; } = null!;

		/// <summary>
		/// Indicates the oldest version of the tool which is still fully compatible with the website.
		/// </summary>
		public Version VersionNumberRequired { get; set; } = null!;

		public IReadOnlyList<ChangelogEntry> Changelog { get; init; } = null!;
	}
}
