using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.CustomLeaderboards.LeaderboardList;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Main;

public static class MainWindow
{
	private static Action? _hoveredButtonAction;

	public static void Render(out bool shouldClose)
	{
		shouldClose = false;

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		Vector2 windowSize = new(683, 768);
		Vector2 mainButtonsSize = new(208, 512);
		Vector2 previewSize = new(windowSize.X - mainButtonsSize.X - 16, 512);

		ImGui.SetNextWindowPos(center, ImGuiCond.Always, new(0.5f, 0.5f));
		ImGui.SetNextWindowSize(windowSize);
		ImGui.SetNextWindowBgAlpha(0.8f);

		ImGui.Begin("Main Menu", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBringToFrontOnFocus);

		ImGui.PushFont(Root.FontGoetheBold60);
		const string title = "ddinfo tools";
		ImGui.TextColored(Vector4.Lerp(Color.Red, Color.Orange, MathF.Sin((float)Root.Window.Time)), title);
		float textWidth = ImGui.CalcTextSize(title).X;
		ImGui.PopFont();

		ImGui.SetCursorPos(new(textWidth + 16, 39));
		ImGui.Text($"{AssemblyUtils.EntryAssemblyVersion} (ALPHA)");
		ImGui.Text("Developed by Noah Stolk");

		ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 8);
		Vector2 iconSize = new(36);
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.ConfigurationTexture.Handle, iconSize))
			UiRenderer.Layout = LayoutType.Config;

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.SettingsTexture.Handle, iconSize))
			UiRenderer.ShowSettings();

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.InfoTexture.Handle, iconSize))
			UiRenderer.ShowAbout();

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.CloseTexture.Handle, iconSize))
			shouldClose = true;

		ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 8);
		if (ImGui.BeginChild("Tool buttons", mainButtonsSize))
		{
			const byte buttonAlpha = 127;
			const float buttonColorDesaturation = 0.3f;
			ToolButton(Colors.SpawnsetEditor.Primary.Desaturate(buttonColorDesaturation) with { A = buttonAlpha }, "Spawnset Editor (wip)", GoToSpawnsetEditor, ref _hoveredButtonAction, RenderSpawnsetEditorPreview);
			ToolButton(Colors.AssetEditor.Desaturate(buttonColorDesaturation) with { A = buttonAlpha }, "Asset Editor (todo)", () => { }, ref _hoveredButtonAction, RenderAssetEditorPreview);
			ToolButton(Colors.ReplayEditor.Primary.Desaturate(buttonColorDesaturation) with { A = buttonAlpha }, "Replay Editor (wip)", GoToReplayEditor, ref _hoveredButtonAction, RenderReplayEditorPreview);
			ToolButton(Colors.CustomLeaderboards.Primary.Desaturate(buttonColorDesaturation) with { A = buttonAlpha }, "Custom Leaderboards", GoToCustomLeaderboards, ref _hoveredButtonAction, RenderCustomLeaderboardsPreview);
			ToolButton(Colors.Practice.Primary.Desaturate(buttonColorDesaturation) with { A = buttonAlpha }, "Practice (wip)", GoToPractice, ref _hoveredButtonAction, RenderPracticePreview);
			ToolButton(Colors.ModManager.Desaturate(buttonColorDesaturation) with { A = buttonAlpha }, "Mod Manager (todo)", () => { }, ref _hoveredButtonAction, RenderModManagerPreview);

			static void GoToSpawnsetEditor() => UiRenderer.Layout = LayoutType.SpawnsetEditor;
			static void GoToReplayEditor() => UiRenderer.Layout = LayoutType.ReplayEditor;
			static void GoToPractice() => UiRenderer.Layout = LayoutType.Practice;

			static void GoToCustomLeaderboards()
			{
				UiRenderer.Layout = LayoutType.CustomLeaderboards;
				LeaderboardListChild.LoadAll();
			}
		}

		ImGui.EndChild();

		if (_hoveredButtonAction != null)
		{
			ImGui.SameLine();
			if (ImGui.BeginChild("Preview", previewSize))
			{
				ImGui.PushTextWrapPos(previewSize.X - 16);
				_hoveredButtonAction.Invoke();
				ImGui.PopTextWrapPos();
			}

			ImGui.EndChild();
		}

		ImGui.End();
	}

	private static void ToolButton(Color color, string text, Action action, ref Action? hoveredAction, Action onHover)
	{
		ImGui.PushStyleColor(ImGuiCol.Button, color);
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color + new Vector4(0, 0, 0, 0.2f));
		ImGui.PushStyleColor(ImGuiCol.ButtonActive, color + new Vector4(0, 0, 0, 0.3f));
		ImGui.PushStyleColor(ImGuiCol.Border, color with { A = 255 });
		ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 4);

		bool clicked = ImGui.Button(text, new(198, 48));

		ImGui.PopStyleColor(4);
		ImGui.PopStyleVar();

		if (ImGui.IsItemHovered())
			hoveredAction = onHover;

		if (clicked)
			action.Invoke();

		ImGui.Spacing();
	}

	private static void RenderSpawnsetEditorPreview()
	{
		ImGuiExt.Title("Spawnset Editor");
		ImGui.Text("""
			Create and edit custom spawnsets (levels) for Devil Daggers.

			Some things you can do:
			- Create your own set of enemy spawns.
			- Create a custom arena.
			- Start with any hand upgrade.
			- Give yourself 10,000 homing daggers.
			- Use the Time Attack game mode, where the goal is to kill all enemies as fast as possible.
			- Use the Race game mode, where the goal is to reach the dagger as fast as possible.

			Be sure to check out the custom leaderboards to see what's possible.

			Note that using custom spawnsets will not submit your score to the official leaderboards.
			Spawnsets can only be used to practice, not cheat. They're completely safe to use.
			""");
	}

	private static void RenderAssetEditorPreview()
	{
		ImGuiExt.Title("Asset Editor");
		ImGui.Text("TODO");
	}

	private static void RenderReplayEditorPreview()
	{
		ImGuiExt.Title("Replay Editor");
		ImGui.Text("WIP");
	}

	private static void RenderCustomLeaderboardsPreview()
	{
		ImGuiExt.Title("Custom Leaderboards");
		ImGui.Text("WIP");
	}

	private static void RenderPracticePreview()
	{
		ImGuiExt.Title("Practice");
		ImGui.Text("WIP");
	}

	private static void RenderModManagerPreview()
	{
		ImGuiExt.Title("Mod Manager");
		ImGui.Text("TODO");
	}
}
