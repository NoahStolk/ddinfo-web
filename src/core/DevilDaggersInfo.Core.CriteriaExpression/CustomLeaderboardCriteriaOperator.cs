namespace DevilDaggersInfo.Core.CriteriaExpression;

// IMPORTANT: These values are stored in the database and should not be changed.
public enum CustomLeaderboardCriteriaOperator : byte
{
	Any = 0,
	Equal = 1,
	LessThan = 2,
	GreaterThan = 3,
	LessThanOrEqual = 4,
	GreaterThanOrEqual = 5,
	Modulo = 6,
	NotEqual = 7,
}
