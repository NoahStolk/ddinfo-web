using DevilDaggersInfo.App.Engine.Maths;
using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Extensions;
using DevilDaggersInfo.App.User.Settings;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.View;
using DevilDaggersInfo.Core.Wiki;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Practice;

public static class PracticeWindow
{
	private const int _templateWidth = 360;
	private const int _templateListWidth = 380;
	private const int _templateContainerWidth = 400;

	private static readonly List<NoFarmTemplate> _noFarmTemplates = new()
	{
		new("First Spider I & Squid II", EnemiesV3_2.Squid2.Color.ToEngineColor(), HandLevel.Level1, 8, 39),
		new("First Centipede", EnemiesV3_2.Centipede.Color.ToEngineColor(), HandLevel.Level2, 35, 114),
		new("Centipede & first triple Spider Is", EnemiesV3_2.Spider1.Color.ToEngineColor(), HandLevel.Level3, 11, 174),
		new("Six Squid Is", EnemiesV3_2.Squid1.Color.ToEngineColor(), HandLevel.Level3, 57, 229),
		new("Gigapedes", EnemiesV3_2.Gigapede.Color.ToEngineColor(), HandLevel.Level3, 81, 259),
		new("Leviathan", EnemiesV3_2.Leviathan.Color.ToEngineColor(), HandLevel.Level4, 105, 350),
		new("Post Orb", EnemiesV3_2.TheOrb.Color.ToEngineColor(), HandLevel.Level4, 129, 397),
	};

	private static readonly List<float> _endLoopTimerStarts = new();

	private static State _state;

	static PracticeWindow()
	{
		const int endLoopTemplateWaveCount = 33;
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

		ImGui.Text("Use these templates to practice specific sections of the game. Click on a template to apply its values.");
		ImGui.Spacing();

		ImGui.BeginChild("No farm templates", new(_templateContainerWidth, 480), true);
		ImGui.Text("No farm templates");

		ImGui.PushTextWrapPos(ImGui.GetCursorPos().X + _templateWidth);
		ImGui.Indent(8);

		ImGui.Spacing();
		ImGui.Text("The amount of gems is based on how many gems you would have at that point, without farming, without losing gems, and without homing usage.");
		ImGui.Spacing();

		ImGui.Indent(-8);
		ImGui.PopTextWrapPos();

		ImGui.BeginChild("No farm template list", new(_templateListWidth, 360));
		foreach (NoFarmTemplate noFarmTemplate in _noFarmTemplates)
			RenderNoFarmTemplate(noFarmTemplate);

		ImGui.EndChild();
		ImGui.EndChild();

		ImGui.SameLine();
		ImGui.BeginChild("End loop templates", new(_templateContainerWidth, 480), true);
		ImGui.Text("End loop templates");

		ImGui.PushTextWrapPos(ImGui.GetCursorPos().X + _templateWidth);
		ImGui.Indent(8);

		ImGui.Spacing();
		ImGui.Text("The amount of homing for the end loop waves is set to 0. Use one that is realistic for you.");
		ImGui.Spacing();

		ImGui.Indent(-8);
		ImGui.PopTextWrapPos();

		ImGui.BeginChild("End loop template list", new(_templateListWidth, 360));
		for (int i = 0; i < _endLoopTimerStarts.Count; i++)
			RenderEndLoopTemplate(i, _endLoopTimerStarts[i]);

		ImGui.EndChild();
		ImGui.EndChild();

		ImGui.BeginChild("Input values", new(512, 192));
		foreach (HandLevel level in Enum.GetValues<HandLevel>())
		{
			if (ImGui.RadioButton($"Lvl {(int)level}", level == _state.HandLevel) && _state.HandLevel != level)
				_state.HandLevel = level;

			if (level != HandLevel.Level4)
				ImGui.SameLine();
		}

		ImGui.InputInt("Added gems", ref _state.AdditionalGems, 1);
		ImGui.InputFloat("Timer start", ref _state.TimerStart, 1, 5, "%.4f");

		if (ImGui.Button("Apply"))
		{
			_state.TimerStart = Math.Clamp(_state.TimerStart, 0, 1400);

			SpawnsetBinary spawnset = ContentManager.Content.DefaultSpawnset;
			float shrinkStart = MathUtils.Lerp(spawnset.ShrinkStart, spawnset.ShrinkEnd, _state.TimerStart / ((spawnset.ShrinkStart - spawnset.ShrinkEnd) / spawnset.ShrinkRate));

			SpawnsetBinary generatedSpawnset = spawnset.GetWithHardcodedEndLoop(70).GetWithTrimmedStart(_state.TimerStart) with
			{
				HandLevel = _state.HandLevel,
				AdditionalGems = _state.AdditionalGems,
				TimerStart = _state.TimerStart,
				SpawnVersion = 6,
				ShrinkStart = shrinkStart,
			};
			File.WriteAllBytes(UserSettings.ModsSurvivalPath, generatedSpawnset.ToBytes());
		}

		ImGui.EndChild();

		ImGui.End();

		if (ImGui.IsKeyPressed(ImGuiKey.Escape) || ImGui.IsKeyPressed((ImGuiKey)526))
			UiRenderer.Layout = LayoutType.Main;
	}

	private static void RenderNoFarmTemplate(NoFarmTemplate noFarmTemplate)
	{
		(byte backgroundAlpha, byte textAlpha) = GetAlpha(noFarmTemplate.IsEqual(_state));

		ImGui.PushStyleColor(ImGuiCol.ChildBg, noFarmTemplate.Color with { A = backgroundAlpha });
		if (ImGui.BeginChild(noFarmTemplate.Name, new(_templateWidth, 48), true))
		{
			bool hover = ImGui.IsWindowHovered();
			if (hover && ImGui.IsMouseReleased(ImGuiMouseButton.Left))
				_state = new(noFarmTemplate.HandLevel, noFarmTemplate.AdditionalGems, noFarmTemplate.TimerStart);

			string timerText = noFarmTemplate.TimerStart.ToString(StringFormats.TimeFormat);
			(string gemsOrHomingText, Color gemColor) = noFarmTemplate.HandLevel switch
			{
				HandLevel.Level3 => ($"{noFarmTemplate.AdditionalGems} homing", HandLevel.Level3.GetColor()),
				HandLevel.Level4 => ($"{noFarmTemplate.AdditionalGems} homing", HandLevel.Level4.GetColor()),
				_ => ($"{noFarmTemplate.AdditionalGems} gems", Color.Red),
			};
			float windowWidth = ImGui.GetWindowWidth();

			ImGui.TextColored(noFarmTemplate.Color with { A = textAlpha }, noFarmTemplate.Name);
			ImGui.SameLine(windowWidth - ImGui.CalcTextSize(timerText).X - 8);
			ImGui.TextColored(Color.White with { A = textAlpha }, timerText);

			ImGui.TextColored(noFarmTemplate.HandLevel.GetColor() with { A = textAlpha }, noFarmTemplate.HandLevel.ToString());
			ImGui.SameLine(windowWidth - ImGui.CalcTextSize(gemsOrHomingText).X - 8);
			ImGui.TextColored(gemColor with { A = textAlpha }, gemsOrHomingText);
		}

		ImGui.EndChild();
		ImGui.PopStyleColor();
	}

	private static void RenderEndLoopTemplate(int waveIndex, float timerStart)
	{
		(byte backgroundAlpha, byte textAlpha) = GetAlpha(IsEqual(_state, timerStart));

		string name = $"Wave {waveIndex + 1}";
		Color color = waveIndex % 3 == 2 ? EnemiesV3_2.Ghostpede.Color.ToEngineColor() : EnemiesV3_2.Gigapede.Color.ToEngineColor();
		ImGui.PushStyleColor(ImGuiCol.ChildBg, color with { A = backgroundAlpha });
		if (ImGui.BeginChild(name, new(_templateWidth, 30), true))
		{
			bool hover = ImGui.IsWindowHovered();
			if (hover && ImGui.IsMouseReleased(ImGuiMouseButton.Left))
				_state = new(HandLevel.Level4, 0, timerStart);

			string timerText = timerStart.ToString(StringFormats.TimeFormat);
			float windowWidth = ImGui.GetWindowWidth();

			ImGui.TextColored(color with { A = textAlpha }, name);
			ImGui.SameLine(windowWidth - ImGui.CalcTextSize(timerText).X - 8);
			ImGui.TextColored(Color.White with { A = textAlpha }, timerText);
		}

		ImGui.EndChild();
		ImGui.PopStyleColor();

		static bool IsEqual(State state, float timerStart)
		{
			return state is { HandLevel: HandLevel.Level4, AdditionalGems: 0 } && Math.Abs(state.TimerStart - timerStart) < 0.00001f;
		}
	}

	private static (byte BackgroundAlpha, byte TextAlpha) GetAlpha(bool isActive)
	{
		return isActive ? ((byte)48, (byte)255) : ((byte)16, (byte)159);
	}

	private struct State
	{
		public HandLevel HandLevel;
		public int AdditionalGems;
		public float TimerStart;

		public State(HandLevel handLevel, int additionalGems, float timerStart)
		{
			HandLevel = handLevel;
			AdditionalGems = additionalGems;
			TimerStart = timerStart;
		}
	}

	private sealed record NoFarmTemplate(string Name, Color Color, HandLevel HandLevel, int AdditionalGems, float TimerStart)
	{
		public bool IsEqual(State state)
		{
			return HandLevel == state.HandLevel && AdditionalGems == state.AdditionalGems && Math.Abs(TimerStart - state.TimerStart) < 0.00001f;
		}
	}
}
