namespace DevilDaggersInfo.Types.Web;

// Note; do not use values 0, 5, and 6, as they have been removed. Re-using these might result in unexpected behavior when this type is exposed to clients using the API.
public enum CustomLeaderboardCategory
{
	Survival = 1,
	TimeAttack = 2,
	Speedrun = 3,
	Race = 4,
}
