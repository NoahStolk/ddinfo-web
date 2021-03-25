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
			StringBuilder sb = new();
			sb.AppendFormat("{0,-22}", nameof(Category)).AppendLine(Category.ToString());
			sb.AppendFormat("{0,-22}", nameof(SpawnsetFileId)).AppendLine(SpawnsetFileId.ToString());
			sb.AppendFormat("{0,-22}", nameof(TimeBronze)).AppendLine(TimeBronze.ToString());
			sb.AppendFormat("{0,-22}", nameof(TimeSilver)).AppendLine(TimeSilver.ToString());
			sb.AppendFormat("{0,-22}", nameof(TimeGolden)).AppendLine(TimeGolden.ToString());
			sb.AppendFormat("{0,-22}", nameof(TimeDevil)).AppendLine(TimeDevil.ToString());
			sb.AppendFormat("{0,-22}", nameof(TimeLeviathan)).AppendLine(TimeLeviathan.ToString());
			return sb.ToString();
		}
	}
}
