using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;
using System.Text;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminAssetMod
	{
		public List<int>? PlayerIds { get; init; }
		public List<AssetModTypes> AssetModTypes { get; init; } = null!;
		public List<AssetModFileContents> AssetModFileContents { get; init; } = null!;
		public string Name { get; init; } = null!;
		public string Url { get; init; } = null!;
		public bool IsHidden { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new("```\n");
			sb.AppendFormat("{0,-30}", nameof(PlayerIds)).AppendLine(PlayerIds != null ? string.Join(", ", PlayerIds) : "Empty");
			sb.AppendFormat("{0,-30}", nameof(AssetModTypes)).AppendLine(string.Join(", ", AssetModTypes));
			sb.AppendFormat("{0,-30}", nameof(AssetModFileContents)).AppendLine(string.Join(", ", AssetModFileContents));
			sb.AppendFormat("{0,-30}", nameof(Name)).AppendLine(Name);
			sb.AppendFormat("{0,-30}", nameof(Url)).AppendLine(Url);
			sb.AppendFormat("{0,-30}", nameof(IsHidden)).AppendLine(IsHidden.ToString());
			return sb.AppendLine("```").ToString();
		}
	}
}
