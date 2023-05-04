using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Extensions;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SettingsChild
{
	public static void Render()
	{
		ImGui.BeginChild("SettingsChild", new(288, 384));

		RenderFormat();
		ImGui.Spacing();

		RenderArena();
		ImGui.Spacing();

		RenderPractice();

		ImGui.Unindent();
		ImGui.EndChild();
	}

	private static void RenderFormat()
	{
		ImGui.Text("Format");
		ImGui.Separator();
		ImGui.Indent(8);

		ImGui.Text("World version");
		ImGui.SameLine();
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

		ImGui.Text("Spawn version");
		ImGui.SameLine();
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

		ImGui.Text("Supported in game version:");

		GameVersion minimumGameVersion;
		if (SpawnsetState.Spawnset.SpawnVersion >= 5)
			minimumGameVersion = GameVersion.V3_1;
		else if (SpawnsetState.Spawnset.WorldVersion >= 9)
			minimumGameVersion = GameVersion.V2_0;
		else
			minimumGameVersion = GameVersion.V1_0;

		ImGui.Text(minimumGameVersion.ToDisplayString());
		ImGui.SameLine();
		ImGui.Text("and newer");
	}

	private static void RenderArena()
	{
		ImGui.Spacing();
		ImGui.Indent(-8);
		ImGui.Text("Arena");
		ImGui.Separator();
		ImGui.Indent(8);

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
	}

	private static void RenderPractice()
	{
		ImGui.BeginDisabled(SpawnsetState.Spawnset.SpawnVersion <= 4);

		ImGui.Spacing();
		ImGui.Indent(-8);
		ImGui.Text("Practice");
		ImGui.Separator();
		ImGui.Indent(8);

		foreach (HandLevel level in Enum.GetValues<HandLevel>())
		{
			if (ImGui.RadioButton($"Lvl {(int)level}", level == SpawnsetState.Spawnset.HandLevel) && SpawnsetState.Spawnset.HandLevel != level)
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

		ImGui.Unindent();
	}
}
