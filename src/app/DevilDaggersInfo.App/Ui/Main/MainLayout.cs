using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Scenes;
using DevilDaggersInfo.App.Ui.CustomLeaderboards.LeaderboardList;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using Silk.NET.OpenGL;
using System.Diagnostics;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Main;

public static class MainLayout
{
	private static readonly string _version = AssemblyUtils.EntryAssemblyVersion;

	private static readonly SpawnsetBinary _mainMenuSpawnset = SpawnsetBinary.CreateDefault();

	private static ArenaScene? _mainMenuScene;

	public static void InitializeScene()
	{
		_mainMenuScene = new(static () => _mainMenuSpawnset, true, false);
		_mainMenuScene.AddSkull4();
	}

	public static void Render(float delta, out bool shouldClose)
	{
		shouldClose = false;

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		ImGui.SetNextWindowPos(center, ImGuiCond.Always, new(0.5f, 0.5f));
		ImGui.SetNextWindowSize(Constants.LayoutSize);

		ImGui.Begin("Main Menu", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBringToFrontOnFocus);

		ImGui.PushFont(Root.FontGoetheBold);
		ImGui.Text("ddinfo tools " + _version);
		ImGui.PopFont();
		if (ImGui.IsItemHovered())
			ImGui.SetTooltip(StringResources.MainMenu);

		ImGui.Text("Devil Daggers is created by Sorath");
		ImGui.Text("DevilDaggers.info is created by Noah Stolk");

		const byte buttonAlpha = 127;
		Color gray = new(96, 96, 96, buttonAlpha);

		MainButton(Colors.SpawnsetEditor.Primary with { A = buttonAlpha }, "Spawnset Editor (wip)", GoToSpawnsetEditor);
		MainButton(Colors.AssetEditor with { A = buttonAlpha }, "Asset Editor (todo)", () => { });
		MainButton(Colors.ReplayEditor.Primary with { A = buttonAlpha }, "Replay Editor (wip)", GoToReplayEditor);
		MainButton(Colors.CustomLeaderboards.Primary with { A = buttonAlpha }, "Custom Leaderboards", GoToCustomLeaderboards);
		MainButton(Colors.Practice.Primary with { A = buttonAlpha }, "Practice (wip)", GoToPractice);
		MainButton(Colors.ModManager with { A = buttonAlpha }, "Mod Manager (todo)", () => { });
		MainButton(gray, "Configuration", GoToConfiguration);
		MainButton(gray, "Settings", UiRenderer.ShowSettings);

		bool close = false; // Must declare another local because lambda capture cannot work with out parameter.
		MainButton(gray, "Exit", () => close = true);
		shouldClose = close;

		Hyperlink("https://devildaggers.info/");
		Hyperlink("https://devildaggers.info/tools");
		Hyperlink("https://github.com/NoahStolk/DevilDaggersInfo");

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

	private static void MainButton(Color color, string text, Action action)
	{
		ImGui.PushStyleColor(ImGuiCol.Button, color);
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color + new Vector4(0, 0, 0, 0.2f));
		ImGui.PushStyleColor(ImGuiCol.ButtonActive, color + new Vector4(0, 0, 0, 0.3f));
		ImGui.PushStyleColor(ImGuiCol.Border, color with { A = 255 });

		ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 4);
		if (ImGui.Button(text, new(192, 48)))
			action.Invoke();

		ImGui.PopStyleVar();

		ImGui.PopStyleColor(4);
	}

	private static void Hyperlink(string url)
	{
		Vector4 hyperlinkColor = new(0, 0.625f, 1, 1);
		Vector4 hyperlinkHoverColor = new(0.25f, 0.875f, 1, 0.25f);

		ImGui.PushStyleColor(ImGuiCol.Text, hyperlinkColor);
		ImGui.PushStyleColor(ImGuiCol.Button, default(Vector4));
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, hyperlinkHoverColor);
		ImGui.PushStyleColor(ImGuiCol.ButtonActive, default(Vector4));

		if (ImGui.Button(url))
			Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

		ImGui.PopStyleColor(4);
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
