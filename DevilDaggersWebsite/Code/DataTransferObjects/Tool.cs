using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Tool
	{
		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		public string DisplayName { get; set; }

		/// <summary>
		/// Indicates the current version of the tool on the website.
		/// </summary>
		[JsonProperty]
		public Version VersionNumber { get; set; }

		/// <summary>
		/// Indicates the oldest version of the tool which is still fully compatible with the website.
		/// </summary>
		[JsonProperty]
		public Version VersionNumberRequired { get; set; }

		[JsonProperty]
		public IReadOnlyList<ChangelogEntry> Changelog { get; set; }
	}
}