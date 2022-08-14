namespace DevilDaggersInfo.Types.Web;

public enum CustomLeaderboardCriteriaOperator : byte
{
	Any = 0,
	Equal = 1,
	LessThan = 2,
	GreaterThan = 3,
	LessThanOrEqual = 4,
	GreaterThanOrEqual = 5,
	Modulo = 6,
}
