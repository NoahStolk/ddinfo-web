using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Tools
{
	public class GetChangePublic
	{
		public GetChangePublic(string description)
		{
			Description = description;
		}

		public string Description { get; }

		public IReadOnlyList<GetChangePublic>? SubChanges { get; init; }

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
