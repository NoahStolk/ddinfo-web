using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevilDaggersWebsite.Core.Dto
{
	public class Tool
	{
		public string Name { get; set; }

		public string DisplayName { get; set; }

		/// <summary>
		/// Indicates the current version of the tool on the website.
		/// </summary>
		public Version VersionNumber { get; set; }

		/// <summary>
		/// Indicates the oldest version of the tool which is still fully compatible with the website.
		/// </summary>
		public Version VersionNumberRequired { get; set; }

		public IReadOnlyList<ChangelogEntry> Changelog { get; set; }

		public HtmlString ToChangelogHtmlString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (ChangelogEntry entry in Changelog)
			{
				sb.Append($"<h3>{entry.VersionNumber} - {entry.Date:MMMM dd, yyyy}</h3><ul>");
				foreach (Change change in entry.Changes)
					sb.Append(change.ToHtmlString());
				sb.Append("</ul>");
			}

			return new HtmlString(sb.ToString());
		}
	}
}