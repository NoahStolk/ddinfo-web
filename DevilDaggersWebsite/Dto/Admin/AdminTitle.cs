using System.Collections.Generic;
using System.Text;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminTitle
	{
		public string Name { get; init; } = null!;
		public List<int>? PlayerIds { get; init; }

		public override string ToString()
		{
			StringBuilder sb = new();
			sb.AppendFormat("{0,-22}", nameof(Name)).AppendLine(Name);
			if (PlayerIds != null)
				sb.AppendFormat("{0,-22}", nameof(PlayerIds)).AppendLine(string.Join(", ", PlayerIds));
			return sb.ToString();
		}
	}
}
