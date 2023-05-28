using DevilDaggersInfo.App.Engine.Maths;
using DevilDaggersInfo.App.User.Settings;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Practice;

public static class PracticeWindow
{
	private static HandLevel _handLevel;
	private static int _additionalGems;
	private static float _timerStart;

	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize);
		ImGui.Begin("Practice", ImGuiWindowFlags.NoCollapse);
		ImGui.PopStyleVar();

		foreach (HandLevel level in Enum.GetValues<HandLevel>())
		{
			if (ImGui.RadioButton($"Lvl {(int)level}", level == _handLevel) && _handLevel != level)
				_handLevel = level;

			if (level != HandLevel.Level4)
				ImGui.SameLine();
		}

		ImGui.InputInt("Added gems", ref _additionalGems, 1);
		ImGui.InputFloat("Timer start", ref _timerStart, 1, 5, "%.4f");

		if (ImGui.Button("Apply"))
		{
			SpawnsetBinary spawnset = ContentManager.Content.DefaultSpawnset;
			float shrinkStart = MathUtils.Lerp(spawnset.ShrinkStart, spawnset.ShrinkEnd, _timerStart / ((spawnset.ShrinkStart - spawnset.ShrinkEnd) / spawnset.ShrinkRate));

			SpawnsetBinary generatedSpawnset = spawnset.GetWithHardcodedEndLoop(50).GetWithTrimmedStart(_timerStart) with
			{
				HandLevel = _handLevel,
				AdditionalGems = _additionalGems,
				TimerStart = _timerStart,
				SpawnVersion = 6,
				ShrinkStart = shrinkStart,
			};
			File.WriteAllBytes(UserSettings.ModsSurvivalPath, generatedSpawnset.ToBytes());
		}

		ImGui.End();

		if (ImGui.IsKeyPressed(ImGuiKey.Escape) || ImGui.IsKeyPressed((ImGuiKey)526))
			UiRenderer.Layout = LayoutType.Main;
	}
}
