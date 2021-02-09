using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Text;

namespace DevilDaggersWebsite.Dto
{
	public class Change
	{
		public Change(string description)
		{
			Description = description;
		}

		public string Description { get; set; }

		public IReadOnlyList<Change>? SubChanges { get; set; }

		public HtmlString ToHtmlString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<li>").Append(Description).Append("</li>");
			if (SubChanges != null && SubChanges.Count != 0)
			{
				foreach (Change subChange in SubChanges)
					sb.Append("<ul>").Append(subChange.ToHtmlString()).Append("</ul>");
			}

			return new HtmlString(sb.ToString());
		}
	}
}
