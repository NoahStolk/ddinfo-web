using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Scenes;
using DevilDaggersInfo.App.Ui.CustomLeaderboards.LeaderboardList;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Main;

public static class MainLayout
{
	private static readonly SpawnsetBinary _mainMenuSpawnset = SpawnsetBinary.CreateDefault();

	private static ArenaScene? _mainMenuScene;
	private static Action? _hoveredButtonAction;

	public static void InitializeScene()
	{
		_mainMenuScene = new(static () => _mainMenuSpawnset, true, false);
		_mainMenuScene.AddSkull4();
	}

	public static void Render(float delta, out bool shouldClose)
	{
		shouldClose = false;

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		Vector2 windowSize = new(683, 768);

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
		if (ImGui.IsItemHovered())
		{
			string tooltip = $"""
				Build time: {AssemblyUtils.EntryAssemblyBuildTime}

				{StringResources.AppDescription}
				""";
			ImGui.SetTooltip(tooltip);
		}

		ImGui.Text("Created by Noah Stolk");

		ImGuiExt.Hyperlink("https://devildaggers.info/");
		ImGuiExt.Hyperlink("https://devildaggers.info/tools");
		ImGuiExt.Hyperlink("https://github.com/NoahStolk/DevilDaggersInfo");
		ImGui.Spacing();

		Vector2 iconSize = new(36);
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.ConfigurationTexture.Handle, iconSize))
			GoToConfiguration();

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.SettingsTexture.Handle, iconSize))
			UiRenderer.ShowSettings();

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.InfoTexture.Handle, iconSize))
			UiRenderer.ShowAbout();

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.CloseTexture.Handle, iconSize))
			shouldClose = true;

		_hoveredButtonAction = null;

		if (ImGui.BeginChild("Main buttons", new(192 + 8, (48 + 4) * 6)))
		{
			const byte buttonAlpha = 127;
			MainButton(Colors.SpawnsetEditor.Primary with { A = buttonAlpha }, "Spawnset Editor (wip)", GoToSpawnsetEditor, ref _hoveredButtonAction, RenderSpawnsetEditorPreview);
			MainButton(Colors.AssetEditor with { A = buttonAlpha }, "Asset Editor (todo)", () => { }, ref _hoveredButtonAction, RenderAssetEditorPreview);
			MainButton(Colors.ReplayEditor.Primary with { A = buttonAlpha }, "Replay Editor (wip)", GoToReplayEditor, ref _hoveredButtonAction, RenderReplayEditorPreview);
			MainButton(Colors.CustomLeaderboards.Primary with { A = buttonAlpha }, "Custom Leaderboards", GoToCustomLeaderboards, ref _hoveredButtonAction, RenderCustomLeaderboardsPreview);
			MainButton(Colors.Practice.Primary with { A = buttonAlpha }, "Practice (wip)", GoToPractice, ref _hoveredButtonAction, RenderPracticePreview);
			MainButton(Colors.ModManager with { A = buttonAlpha }, "Mod Manager (todo)", () => { }, ref _hoveredButtonAction, RenderModManagerPreview);
		}

		ImGui.EndChild();

		if (_hoveredButtonAction != null)
		{
			ImGui.SameLine();
			if (ImGui.BeginChild("Preview", new(512, 256)))
			{
				_hoveredButtonAction();
			}

			ImGui.EndChild();
		}

		ImGui.End();

		RenderScene(delta);
	}

	private static void GoToSpawnsetEditor()
	{
		UiRenderer.Layout = LayoutType.SpawnsetEditor;
		Colors.SetColors(Colors.SpawnsetEditor);
	}

	private static void GoToReplayEditor()
	{
		UiRenderer.Layout = LayoutType.ReplayEditor;
		Colors.SetColors(Colors.ReplayEditor);
	}

	private static void GoToCustomLeaderboards()
	{
		UiRenderer.Layout = LayoutType.CustomLeaderboards;
		Colors.SetColors(Colors.CustomLeaderboards);
		LeaderboardListChild.LoadAll();
	}

	private static void GoToPractice()
	{
		UiRenderer.Layout = LayoutType.Practice;
		Colors.SetColors(Colors.Practice);
	}

	private static void GoToConfiguration()
	{
		UiRenderer.Layout = LayoutType.Config;
	}

	private static void MainButton(Color color, string text, Action action, ref Action? hoveredAction, Action onHover)
	{
		ImGui.PushStyleColor(ImGuiCol.Button, color);
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color + new Vector4(0, 0, 0, 0.2f));
		ImGui.PushStyleColor(ImGuiCol.ButtonActive, color + new Vector4(0, 0, 0, 0.3f));
		ImGui.PushStyleColor(ImGuiCol.Border, color with { A = 255 });
		ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 4);

		if (ImGui.Button(text, new(192, 48)))
			action.Invoke();

		if (ImGui.IsItemHovered())
			hoveredAction = onHover;

		ImGui.PopStyleVar();
		ImGui.PopStyleColor(4);
	}

	private static void RenderSpawnsetEditorPreview()
	{
		ImGui.Text("Spawnset Editor");
		ImGui.Text("WIP");
	}

	private static void RenderAssetEditorPreview()
	{
		ImGui.Text("Asset Editor");
		ImGui.Text("TODO");
	}

	private static void RenderReplayEditorPreview()
	{
		ImGui.Text("Replay Editor");
		ImGui.Text("WIP");
	}

	private static void RenderCustomLeaderboardsPreview()
	{
		ImGui.Text("Custom Leaderboards");
		ImGui.Text("WIP");
	}

	private static void RenderPracticePreview()
	{
		ImGui.Text("Practice");
		ImGui.Text("WIP");
	}

	private static void RenderModManagerPreview()
	{
		ImGui.Text("Mod Manager");
		ImGui.Text("TODO");
	}

	private static void RenderConfigurationPreview()
	{
		ImGui.Text("Configuration");
		ImGui.Text("TODO");
	}

	private static void RenderSettingsPreview()
	{
		ImGui.Text("Settings");
		ImGui.Text("TODO");
	}

	private static void RenderScene(float delta)
	{
		_mainMenuScene?.Update(false, false, delta);

		Root.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

		int framebufferWidth = Root.Window.Size.X;
		int framebufferHeight = Root.Window.Size.Y;

		// Keep track of the original viewport so we can restore it later.
		Span<int> originalViewport = stackalloc int[4];
		Root.Gl.GetInteger(GLEnum.Viewport, originalViewport);
		Root.Gl.Viewport(0, 0, (uint)framebufferWidth, (uint)framebufferHeight);

		Root.Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		Root.Gl.Enable(EnableCap.DepthTest);
		Root.Gl.Enable(EnableCap.Blend);
		Root.Gl.Enable(EnableCap.CullFace);
		Root.Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		_mainMenuScene?.Render(framebufferWidth, framebufferHeight);

		Root.Gl.Viewport(originalViewport[0], originalViewport[1], (uint)originalViewport[2], (uint)originalViewport[3]);
		Root.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
	}
}
