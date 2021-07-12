using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Tools
{
	public class GetToolPublic
	{
		public string Name { get; init; } = null!;

		public string DisplayName { get; init; } = null!;

		/// <summary>
		/// Indicates the current version of the tool on the website.
		/// </summary>
		public Version VersionNumber { get; init; } = null!;

		/// <summary>
		/// Indicates the oldest version of the tool which is still fully compatible with the website.
		/// </summary>
		public Version VersionNumberRequired { get; init; } = null!;

		public IReadOnlyList<GetChangelogEntryPublic> Changelog { get; init; } = null!;

		//public HtmlString ToChangelogHtmlString()
		//{
		//	StringBuilder sb = new();
		//	foreach (ChangelogEntry entry in Changelog)
		//	{
		//		sb.Append("<h3>").Append(entry.VersionNumber).Append(" - ").AppendFormat("{0:MMMM dd, yyyy}", entry.Date).Append("</h3><ul>");
		//		foreach (Change change in entry.Changes)
		//			sb.Append(change.ToHtmlString());
		//		sb.Append("</ul>");
		//	}

		//	return new(sb.ToString());
		//}
	}
}
