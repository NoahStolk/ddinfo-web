using DevilDaggersInfo.App.Engine.Maths;
using DevilDaggersInfo.App.User.Settings;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.View;
using DevilDaggersInfo.Core.Wiki;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Practice;

public static class PracticeWindow
{
	private static readonly List<NoFarmTemplate> _noFarmTemplates = new()
	{
		new("First Spider I & Squid II", HandLevel.Level1, 8, 39),
		new("First Centipede", HandLevel.Level2, 35, 114),
		new("Centipede and first triple Spider Is", HandLevel.Level3, 11, 174),
		new("Six Squid Is", HandLevel.Level3, 57, 229),
		new("Gigapedes", HandLevel.Level3, 81, 259),
		new("Leviathan", HandLevel.Level4, 105, 350),
		new("Post Orb", HandLevel.Level4, 129, 397),
	};

	private static readonly List<float> _endLoopTimerStarts = new();

	private static HandLevel _handLevel = HandLevel.Level4;
	private static int _additionalGems;
	private static float _timerStart;

	static PracticeWindow()
	{
		const int endLoopTemplateWaveCount = 50;
		SpawnsView spawnsView = new(ContentManager.Content.DefaultSpawnset, GameVersion.V3_2, endLoopTemplateWaveCount);
		for (int i = 0; i < endLoopTemplateWaveCount; i++)
		{
			_endLoopTimerStarts.Add((float)spawnsView.Waves[i][0].Seconds);
		}
	}

	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize);
		ImGui.Begin("Practice", ImGuiWindowFlags.NoCollapse);
		ImGui.PopStyleVar();

		foreach (NoFarmTemplate noFarmTemplate in _noFarmTemplates)
		{
			string name = $"{noFarmTemplate.TimerStart} - {noFarmTemplate.Name}";
			if (ImGui.Button(name))
			{
				_handLevel = noFarmTemplate.HandLevel;
				_additionalGems = noFarmTemplate.AdditionalGems;
				_timerStart = noFarmTemplate.TimerStart;
			}
		}

		foreach (float endLoopTimerStart in _endLoopTimerStarts)
		{
			if (ImGui.Button(endLoopTimerStart.ToString(StringFormats.TimeFormat)))
			{
				_handLevel = HandLevel.Level4;
				_additionalGems = 0;
				_timerStart = endLoopTimerStart;
			}
		}

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
			_timerStart = Math.Clamp(_timerStart, 0, 1400);

			SpawnsetBinary spawnset = ContentManager.Content.DefaultSpawnset;
			float shrinkStart = MathUtils.Lerp(spawnset.ShrinkStart, spawnset.ShrinkEnd, _timerStart / ((spawnset.ShrinkStart - spawnset.ShrinkEnd) / spawnset.ShrinkRate));

			SpawnsetBinary generatedSpawnset = spawnset.GetWithHardcodedEndLoop(70).GetWithTrimmedStart(_timerStart) with
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

	private sealed record NoFarmTemplate(string Name, HandLevel HandLevel, int AdditionalGems, float TimerStart);
}
