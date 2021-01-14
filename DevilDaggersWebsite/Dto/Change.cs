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
			sb.Append($"<li>{Description}</li>");
			if (SubChanges != null && SubChanges.Count != 0)
			{
				foreach (Change subChange in SubChanges)
					sb.Append($"<ul>{subChange.ToHtmlString()}</ul>");
			}

			return new HtmlString(sb.ToString());
		}
	}
}
