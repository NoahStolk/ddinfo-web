using System.Collections.Generic;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools
{
	public class GetChange
	{
		public GetChange(string description)
		{
			Description = description;
		}

		public string Description { get; }

		public IReadOnlyList<GetChange>? SubChanges { get; init; }

		//public HtmlString ToHtmlString()
		//{
		//	StringBuilder sb = new();
		//	sb.Append("<li>").Append(Description).Append("</li>");
		//	if (SubChanges != null && SubChanges.Count != 0)
		//	{
		//		foreach (Change subChange in SubChanges)
		//			sb.Append("<ul>").Append(subChange.ToHtmlString()).Append("</ul>");
		//	}

		//	return new(sb.ToString());
		//}
	}
}
