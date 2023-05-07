namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;

public class LeaderboardListEntry : LeaderboardListRow
{
	private readonly List<CriteriaIcon> _criteriaIcons = new();

	public void SetCustomLeaderboard(GetCustomLeaderboardForOverview? customLeaderboard)
	{
		foreach (CriteriaIcon icon in _criteriaIcons)
			NestingContext.Remove(icon);

		_criteriaIcons.Clear();

		int iconOffset = 0;
		foreach (GetCustomLeaderboardCriteria criteria in customLeaderboard.Criteria)
		{
			CriteriaIcon icon = new(Bounds.CreateNested(GridCriteria.Index + iconOffset, 0, 16, 16), criteria) { Depth = Depth + 1 };
			NestingContext.Add(icon);
			_criteriaIcons.Add(icon);

			iconOffset += 16;
		}
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		StateManager.Dispatch(new SetSelectedCustomLeaderboard(_customLeaderboard.Id, _customLeaderboard.SpawnsetId, _customLeaderboard.SpawnsetName));
	}
}
