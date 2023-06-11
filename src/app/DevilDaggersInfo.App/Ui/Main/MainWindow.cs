using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.CustomLeaderboards.LeaderboardList;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Main;

public static class MainWindow
{
	public static void Render(out bool shouldClose)
	{
		shouldClose = false;

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		Vector2 windowSize = new(683, 768);
		Vector2 mainButtonsSize = new(208, 512);
		Vector2 previewSize = new(windowSize.X - mainButtonsSize.X - 16, 512);

		ImGui.SetNextWindowPos(center, ImGuiCond.Always, new(0.5f, 0.5f));
		ImGui.SetNextWindowSize(windowSize);
		ImGui.SetNextWindowBgAlpha(0.75f);

		ImGui.Begin("Main Menu", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBringToFrontOnFocus);

		ImGui.PushFont(Root.FontGoetheBold60);
		const string title = "ddinfo tools";
		ImGui.TextColored(Vector4.Lerp(Color.Red, Color.Orange, MathF.Sin((float)Root.Window.Time)), title);
		float textWidth = ImGui.CalcTextSize(title).X;
		ImGui.PopFont();

		ImGui.SetCursorPos(new(textWidth + 16, 39));
		ImGui.Text($"{AssemblyUtils.EntryAssemblyVersion} (ALPHA)");
		ImGui.Text("Developed by Noah Stolk");

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

		Action? hoveredButtonAction = null;
		if (ImGui.BeginChild("Main buttons", mainButtonsSize))
		{
			const byte buttonAlpha = 127;
			MainButton(Colors.SpawnsetEditor.Primary with { A = buttonAlpha }, "Spawnset Editor (wip)", GoToSpawnsetEditor, ref hoveredButtonAction, RenderSpawnsetEditorPreview);
			MainButton(Colors.AssetEditor with { A = buttonAlpha }, "Asset Editor (todo)", () => { }, ref hoveredButtonAction, RenderAssetEditorPreview);
			MainButton(Colors.ReplayEditor.Primary with { A = buttonAlpha }, "Replay Editor (wip)", GoToReplayEditor, ref hoveredButtonAction, RenderReplayEditorPreview);
			MainButton(Colors.CustomLeaderboards.Primary with { A = buttonAlpha }, "Custom Leaderboards", GoToCustomLeaderboards, ref hoveredButtonAction, RenderCustomLeaderboardsPreview);
			MainButton(Colors.Practice.Primary with { A = buttonAlpha }, "Practice (wip)", GoToPractice, ref hoveredButtonAction, RenderPracticePreview);
			MainButton(Colors.ModManager with { A = buttonAlpha }, "Mod Manager (todo)", () => { }, ref hoveredButtonAction, RenderModManagerPreview);

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

		if (hoveredButtonAction != null)
		{
			ImGui.SameLine();
			if (ImGui.BeginChild("Preview", previewSize))
			{
				ImGui.PushTextWrapPos(previewSize.X - 16);
				hoveredButtonAction();
				ImGui.PopTextWrapPos();
			}

			ImGui.EndChild();
		}

		ImGui.End();
	}

	private static void MainButton(Color color, string text, Action action, ref Action? hoveredAction, Action onHover)
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
			- Start with any hand upgrade.
			- Create a custom arena made out of towers and gaps.
			- Give yourself 10,000 homing daggers.
			- Use the Time Attack game mode, where the goal is to kill all enemies as fast as possible.
			- Use the Race game mode, where the goal is to reach the dagger as fast as possible.

			Be sure to check out the custom leaderboards to see what's possible.
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
