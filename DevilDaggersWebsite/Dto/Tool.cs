using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevilDaggersWebsite.Dto
{
	public class Tool
	{
		public string Name { get; set; } = null!;

		public string DisplayName { get; set; } = null!;

		/// <summary>
		/// Indicates the current version of the tool on the website.
		/// </summary>
		public Version VersionNumber { get; set; } = null!;

		/// <summary>
		/// Indicates the oldest version of the tool which is still fully compatible with the website.
		/// </summary>
		public Version VersionNumberRequired { get; set; } = null!;

		public IReadOnlyList<ChangelogEntry> Changelog { get; set; } = null!;

		public HtmlString ToChangelogHtmlString()
		{
			StringBuilder sb = new();
			foreach (ChangelogEntry entry in Changelog)
			{
				sb.Append("<h3>").Append(entry.VersionNumber).Append(" - ").AppendFormat("{0:MMMM dd, yyyy}", entry.Date).Append("</h3><ul>");
				foreach (Change change in entry.Changes)
					sb.Append(change.ToHtmlString());
				sb.Append("</ul>");
			}

			return new(sb.ToString());
		}
	}
}
