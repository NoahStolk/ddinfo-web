using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SpawnsetSettingsWindow
{
	public static void Render()
	{
		ImGui.BeginChild("SettingsChild", new(384, 256));

		RenderVersionRadioButtons();

		float shrinkStart = SpawnsetState.Spawnset.ShrinkStart;
		ImGui.InputFloat("Shrink start", ref shrinkStart, 1, 5, "%.1f");
		if (Math.Abs(SpawnsetState.Spawnset.ShrinkStart - shrinkStart) > 0.001f)
		{
			SpawnsetState.Spawnset = SpawnsetState.Spawnset with { ShrinkStart = shrinkStart };
			SpawnsetHistoryUtils.Save(SpawnsetEditType.ShrinkStart);
		}

		float shrinkEnd = SpawnsetState.Spawnset.ShrinkEnd;
		ImGui.InputFloat("Shrink end", ref shrinkEnd, 1, 5, "%.1f");
		if (Math.Abs(SpawnsetState.Spawnset.ShrinkEnd - shrinkEnd) > 0.001f)
		{
			SpawnsetState.Spawnset = SpawnsetState.Spawnset with { ShrinkEnd = shrinkEnd };
			SpawnsetHistoryUtils.Save(SpawnsetEditType.ShrinkEnd);
		}

		float shrinkRate = SpawnsetState.Spawnset.ShrinkRate;
		ImGui.InputFloat("Shrink rate", ref shrinkRate, 0.005f, 0.5f, "%.3f");
		if (Math.Abs(SpawnsetState.Spawnset.ShrinkRate - shrinkRate) > 0.001f)
		{
			SpawnsetState.Spawnset = SpawnsetState.Spawnset with { ShrinkRate = shrinkRate };
			SpawnsetHistoryUtils.Save(SpawnsetEditType.ShrinkRate);
		}

		float brightness = SpawnsetState.Spawnset.Brightness;
		ImGui.InputFloat("Brightness", ref brightness, 5, 20, "%.1f");
		if (Math.Abs(SpawnsetState.Spawnset.Brightness - brightness) > 0.001f)
		{
			SpawnsetState.Spawnset = SpawnsetState.Spawnset with { Brightness = brightness };
			SpawnsetHistoryUtils.Save(SpawnsetEditType.Brightness);
		}

		ImGui.BeginDisabled(SpawnsetState.Spawnset.SpawnVersion <= 4);

		foreach (HandLevel level in Enum.GetValues<HandLevel>())
		{
			if (ImGui.RadioButton(level.ToString(), level == SpawnsetState.Spawnset.HandLevel) && SpawnsetState.Spawnset.HandLevel != level)
			{
				SpawnsetState.Spawnset = SpawnsetState.Spawnset with { HandLevel = level };
				SpawnsetHistoryUtils.Save(SpawnsetEditType.HandLevel);
			}

			if (level != HandLevel.Level4)
				ImGui.SameLine();
		}

		int additionalGems = SpawnsetState.Spawnset.AdditionalGems;
		ImGui.InputInt("Added gems", ref additionalGems, 1);
		if (SpawnsetState.Spawnset.AdditionalGems != additionalGems)
		{
			SpawnsetState.Spawnset = SpawnsetState.Spawnset with { AdditionalGems = additionalGems };
			SpawnsetHistoryUtils.Save(SpawnsetEditType.AdditionalGems);
		}

		ImGui.EndDisabled();

		ImGui.BeginDisabled(SpawnsetState.Spawnset.SpawnVersion <= 5);

		float timerStart = SpawnsetState.Spawnset.TimerStart;
		ImGui.InputFloat("Timer start", ref timerStart, 1, 5, "%.4f");
		if (Math.Abs(SpawnsetState.Spawnset.TimerStart - timerStart) > 0.001f)
		{
			SpawnsetState.Spawnset = SpawnsetState.Spawnset with { TimerStart = timerStart };
			SpawnsetHistoryUtils.Save(SpawnsetEditType.TimerStart);
		}

		ImGui.EndDisabled();

		ImGui.EndChild();
	}

	private static void RenderVersionRadioButtons()
	{
		for (int i = 8; i < 10; i++)
		{
			if (ImGui.RadioButton(i.ToString(), i == SpawnsetState.Spawnset.WorldVersion) && SpawnsetState.Spawnset.WorldVersion != i)
			{
				SpawnsetState.Spawnset = SpawnsetState.Spawnset with { WorldVersion = i };
				SpawnsetHistoryUtils.Save(SpawnsetEditType.Format);
			}

			if (i < 9)
				ImGui.SameLine();
		}

		for (int i = 4; i < 7; i++)
		{
			if (ImGui.RadioButton(i.ToString(), i == SpawnsetState.Spawnset.SpawnVersion) && SpawnsetState.Spawnset.SpawnVersion != i)
			{
				SpawnsetState.Spawnset = SpawnsetState.Spawnset with { SpawnVersion = i };
				SpawnsetHistoryUtils.Save(SpawnsetEditType.Format);
			}

			if (i < 6)
				ImGui.SameLine();
		}

		ImGui.Text(SpawnsetState.Spawnset.GetGameVersionString());
	}
}
