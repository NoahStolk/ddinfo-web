using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Shared
{
	public static class Policies
	{
		public const string AdminPolicy = nameof(AdminPolicy);
		public const string AssetModsPolicy = nameof(AssetModsPolicy);
		public const string CustomLeaderboardsPolicy = nameof(CustomLeaderboardsPolicy);
		public const string DonationsPolicy = nameof(DonationsPolicy);
		public const string PlayersPolicy = nameof(PlayersPolicy);
		public const string SpawnsetsPolicy = nameof(SpawnsetsPolicy);

		private static readonly List<string> _all = typeof(Policies).GetFields().Where(f => f.FieldType == typeof(string)).Select(f => (string)f.GetValue(null)!).ToList();

		public static IReadOnlyList<string> All => _all;
	}
}
