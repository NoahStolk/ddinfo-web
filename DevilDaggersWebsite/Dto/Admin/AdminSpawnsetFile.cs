using System.Text;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminSpawnsetFile
	{
		public string Name { get; init; } = null!;
		public int PlayerId { get; init; }
		public int? MaxDisplayWaves { get; init; }
		public string? HtmlDescription { get; init; }
		public bool IsPractice { get; init; }

		public override string ToString()
		{
			StringBuilder sb = new("```\n");
			sb.AppendFormat("{0,-30}", nameof(Name)).AppendLine(Name);
			sb.AppendFormat("{0,-30}", nameof(PlayerId)).AppendLine(PlayerId.ToString());
			sb.AppendFormat("{0,-30}", nameof(MaxDisplayWaves)).AppendLine(MaxDisplayWaves.ToString());
			sb.AppendFormat("{0,-30}", nameof(HtmlDescription)).AppendLine(HtmlDescription);
			sb.AppendFormat("{0,-30}", nameof(IsPractice)).AppendLine(IsPractice.ToString());
			return sb.AppendLine("```").ToString();
		}
	}
}
