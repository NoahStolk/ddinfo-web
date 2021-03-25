using DevilDaggersWebsite.Enumerators;
using System.Text;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminCustomLeaderboard
	{
		public CustomLeaderboardCategory Category { get; init; }
		public int SpawnsetFileId { get; init; }

		public int TimeBronze { get; init; }
		public int TimeSilver { get; init; }
		public int TimeGolden { get; init; }
		public int TimeDevil { get; init; }
		public int TimeLeviathan { get; init; }

		public override string ToString()
		{
			StringBuilder sb = new("```\n");
			sb.AppendFormat("{0,-30}", nameof(Category)).AppendLine(Category.ToString());
			sb.AppendFormat("{0,-30}", nameof(SpawnsetFileId)).AppendLine(SpawnsetFileId.ToString());
			sb.AppendFormat("{0,-30}", nameof(TimeBronze)).AppendLine(TimeBronze.ToString());
			sb.AppendFormat("{0,-30}", nameof(TimeSilver)).AppendLine(TimeSilver.ToString());
			sb.AppendFormat("{0,-30}", nameof(TimeGolden)).AppendLine(TimeGolden.ToString());
			sb.AppendFormat("{0,-30}", nameof(TimeDevil)).AppendLine(TimeDevil.ToString());
			sb.AppendFormat("{0,-30}", nameof(TimeLeviathan)).AppendLine(TimeLeviathan.ToString());
			return sb.AppendLine("```").ToString();
		}
	}
}
