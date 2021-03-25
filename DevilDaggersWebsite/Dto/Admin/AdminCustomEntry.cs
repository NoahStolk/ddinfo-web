using System;
using System.Text;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminCustomEntry
	{
		public int PlayerId { get; init; }
		public int CustomLeaderboardId { get; init; }

		public int Time { get; init; }
		public int GemsCollected { get; init; }
		public int EnemiesKilled { get; init; }
		public int DaggersFired { get; init; }
		public int DaggersHit { get; init; }
		public int EnemiesAlive { get; init; }
		public int HomingDaggers { get; init; }
		public int HomingDaggersEaten { get; init; }
		public int GemsDespawned { get; init; }
		public int GemsEaten { get; init; }
		public int GemsTotal { get; init; }
		public byte DeathType { get; init; }
		public int LevelUpTime2 { get; init; }
		public int LevelUpTime3 { get; init; }
		public int LevelUpTime4 { get; init; }
		public DateTime SubmitDate { get; init; }
		public string ClientVersion { get; init; } = null!;

		public override string ToString()
		{
			StringBuilder sb = new();
			sb.AppendFormat("{0,-22}", nameof(PlayerId)).AppendLine(PlayerId.ToString());
			sb.AppendFormat("{0,-22}", nameof(CustomLeaderboardId)).AppendLine(CustomLeaderboardId.ToString());
			sb.AppendFormat("{0,-22}", nameof(Time)).AppendLine(Time.ToString());
			sb.AppendFormat("{0,-22}", nameof(GemsCollected)).AppendLine(GemsCollected.ToString());
			sb.AppendFormat("{0,-22}", nameof(EnemiesKilled)).AppendLine(EnemiesKilled.ToString());
			sb.AppendFormat("{0,-22}", nameof(DaggersFired)).AppendLine(DaggersFired.ToString());
			sb.AppendFormat("{0,-22}", nameof(DaggersHit)).AppendLine(DaggersHit.ToString());
			sb.AppendFormat("{0,-22}", nameof(EnemiesAlive)).AppendLine(EnemiesAlive.ToString());
			sb.AppendFormat("{0,-22}", nameof(HomingDaggers)).AppendLine(HomingDaggers.ToString());
			sb.AppendFormat("{0,-22}", nameof(HomingDaggersEaten)).AppendLine(HomingDaggersEaten.ToString());
			sb.AppendFormat("{0,-22}", nameof(GemsDespawned)).AppendLine(GemsDespawned.ToString());
			sb.AppendFormat("{0,-22}", nameof(GemsEaten)).AppendLine(GemsEaten.ToString());
			sb.AppendFormat("{0,-22}", nameof(GemsTotal)).AppendLine(GemsTotal.ToString());
			sb.AppendFormat("{0,-22}", nameof(DeathType)).AppendLine(DeathType.ToString());
			sb.AppendFormat("{0,-22}", nameof(LevelUpTime2)).AppendLine(LevelUpTime2.ToString());
			sb.AppendFormat("{0,-22}", nameof(LevelUpTime3)).AppendLine(LevelUpTime3.ToString());
			sb.AppendFormat("{0,-22}", nameof(LevelUpTime4)).AppendLine(LevelUpTime4.ToString());
			sb.AppendFormat("{0,-22}", nameof(SubmitDate)).AppendLine(SubmitDate.ToString());
			sb.AppendFormat("{0,-22}", nameof(ClientVersion)).AppendLine(ClientVersion.ToString());
			return sb.ToString();
		}
	}
}
