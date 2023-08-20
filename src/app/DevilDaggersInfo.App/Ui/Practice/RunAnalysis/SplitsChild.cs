using DevilDaggersInfo.App.Engine.Maths.Numerics;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Practice.RunAnalysis;

public static class SplitsChild
{
	private static readonly IReadOnlyList<(int Label, int Seconds)> _splitData = new List<(int Label, int Seconds)>
	{
		(350, 366),
		(700, 709),
		(800, 800),
		(880, 875),
		(930, 942),
		(1000, 996),
		(1040, 1047),
		(1080, 1091),
		(1130, 1133),
		(1160, 1170),
	};
	private static readonly (int DisplayTimer, int? Homing, int? HomingPrevious)[] _homingSplitData = new (int DisplayTimer, int? Homing, int? HomingPrevious)[_splitData.Count + 2]; // Include start and end.

	public static void Render()
	{
		// TODO: Move to PracticeStatsData.
		bool addedTimerStart = false;
		bool addedTimerEnd = false;
		int homingSplitDataIndex = 0;
		for (int i = 0; i < _splitData.Count; i++)
		{
			(int Label, int Seconds) splitEntry = _splitData[i];

			if (!addedTimerStart && splitEntry.Seconds > PracticeStatsData.TimerStart)
			{
				int? firstHomingStored = RunAnalysisWindow.StatsData.Statistics.Count > 0 ? RunAnalysisWindow.StatsData.Statistics[0].HomingStored : null;
				addedTimerStart = true;
				_homingSplitData[homingSplitDataIndex] = ((int)PracticeStatsData.TimerStart, firstHomingStored, firstHomingStored);
				homingSplitDataIndex++;
			}
			else if (!addedTimerEnd && splitEntry.Seconds > PracticeStatsData.TimerEnd)
			{
				int? lastHomingStored = RunAnalysisWindow.StatsData.Statistics.Count > 0 ? RunAnalysisWindow.StatsData.Statistics[^1].HomingStored : null;
				addedTimerEnd = true;
				_homingSplitData[homingSplitDataIndex] = ((int)PracticeStatsData.TimerEnd, lastHomingStored, homingSplitDataIndex == 0 ? null : _homingSplitData[homingSplitDataIndex - 1].Homing);
				homingSplitDataIndex++;
			}

			int actualIndex = splitEntry.Seconds - (int)MathF.Ceiling(PracticeStatsData.TimerStart); // TODO: Test if ceiling is correct.
			bool hasValue = actualIndex >= 0 && actualIndex < RunAnalysisWindow.StatsData.Statistics.Count;
			int? homing = hasValue ? RunAnalysisWindow.StatsData.Statistics[actualIndex].HomingStored : null;
			int? previousHoming = homingSplitDataIndex > 0 ? _homingSplitData[homingSplitDataIndex - 1].Homing : null;

			_homingSplitData[homingSplitDataIndex] = (splitEntry.Label, homing, previousHoming);
			homingSplitDataIndex++;
		}

		if (ImGui.BeginChild("Splits", new(192, 224)))
		{
			if (ImGui.BeginTable("LeaderboardTable", 3, ImGuiTableFlags.None))
			{
				ImGui.TableSetupColumn("Split", ImGuiTableColumnFlags.None, 40);
				ImGui.TableSetupColumn("Homing", ImGuiTableColumnFlags.None, 40);
				ImGui.TableSetupColumn("Delta", ImGuiTableColumnFlags.None, 40);
				ImGui.TableHeadersRow();

				for (int i = 0; i < _homingSplitData.Length; i++)
				{
					float displayTimer = _homingSplitData[i].DisplayTimer;
					int? homing = _homingSplitData[i].Homing;
					int? homingPrevious = _homingSplitData[i].HomingPrevious;

					Color textColor = homing.HasValue ? Color.White : Color.Gray(0.5f);

					ImGui.TableNextRow();

					ImGui.TableNextColumn();
					ImGui.TextColored(textColor, UnsafeSpan.Get(displayTimer));

					ImGui.TableNextColumn();
					ImGui.TextColored(textColor, homing.HasValue ? UnsafeSpan.Get(homing.Value) : "-");

					ImGui.TableNextColumn();
					int? delta = homing.HasValue && homingPrevious.HasValue ? homing.Value - homingPrevious.Value : null;
					Color color = delta switch
					{
						< 0 => Color.Red,
						> 0 => Color.Green,
						null => Color.Gray(0.5f),
						_ => Color.White,
					};
					ImGui.TextColored(color, delta.HasValue ? UnsafeSpan.Get(delta.Value, "+0;-0;+0") : "-");
				}

				ImGui.EndTable();
			}

			ImGui.EndChild();
		}
	}
}
