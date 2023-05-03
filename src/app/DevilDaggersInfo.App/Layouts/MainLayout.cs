using DevilDaggersInfo.App.Scenes;
using DevilDaggersInfo.App.Utils;
using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Diagnostics;
using System.Numerics;

namespace DevilDaggersInfo.App.Layouts;

public static class MainLayout
{
	private const string _mainMenu = """
		This is an alpha version of the rewritten tools.
		It is still very much a work in progress.

		I also do not have a deadline or schedule for these developments,
		and there will not be an official release date any time soon.

		If you encounter any problems, please report them on Discord/GitHub.

		Thank you for testing.

		For more information, go to:
		""";

	private static readonly string _version = VersionUtils.EntryAssemblyVersion;

	private static MainMenuArenaScene? _arenaScene;

	private static bool _showSettings;

	public static void Initialize()
	{
		_arenaScene = new();
		_arenaScene.BuildSpawnset(SpawnsetBinary.CreateDefault());
	}

	public static void Update(float delta)
	{
		_arenaScene?.Update(0, delta);
	}

	public static void Render3d()
	{
		_arenaScene?.Render();
	}

	public static void Render(out bool shouldClose)
	{
		shouldClose = false;

		ImGui.SetNextWindowPos(new(0, 0));
		ImGui.SetNextWindowSize(new(1024, 768));

		ImGui.Begin("Main Menu", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus);

		TextAt(_mainMenu, 8, 8);
		TextAt("DDINFO", 512, 64, new(1, 0, 0, 1), true); // TODO: font size 64
		TextAt("TOOLS", 512, 128, new(1, 0.5f, 0, 1), true); // TODO: font size 32
		TextAt(_version, 512, 176, new(1, 0.8f, 0, 1), true); // TODO: font size 24
		TextAt("Devil Daggers is created by Sorath", 512, 708, default, true);
		TextAt("DevilDaggers.info is created by Noah Stolk", 512, 724, default, true);

		const float alpha = 0.5f;
		MainButtonAt(0, 0, new(1, 0, 0, alpha), "Survival Editor (wip)");
		MainButtonAt(1, 0, new(0, 1, 0, alpha), "Asset Editor (todo)");
		MainButtonAt(2, 0, new(1, 0, 1, alpha), "Replay Editor (wip)");
		MainButtonAt(0, 1, new(0, 0, 1, alpha), "Custom Leaderboards");
		MainButtonAt(1, 1, new(0, 1, 1, alpha), "Practice (todo)");
		MainButtonAt(2, 1, new(1, 1, 0, alpha), "Mod Manager (todo)");
		MainButtonAt(0, 2, new(0.3f, 0.3f, 0.3f, alpha), "Configuration");
		if (MainButtonAt(1, 2, new(0.3f, 0.3f, 0.3f, alpha), "Settings"))
			_showSettings = true;

		if (MainButtonAt(2, 2, new(0.3f, 0.3f, 0.3f, alpha), "Exit"))
			shouldClose = true;

		Vector4 hyperlinkColor = new(0, 0.625f, 1, 1);
		Vector4 hyperlinkHoverColor = new(0.25f, 0.875f, 1, 0.25f);

		ImGui.PushStyleColor(ImGuiCol.Text, hyperlinkColor);
		ImGui.PushStyleColor(ImGuiCol.Button, default(Vector4));
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, hyperlinkHoverColor);
		ImGui.PushStyleColor(ImGuiCol.ButtonActive, default(Vector4));

		const string homePage = "https://devildaggers.info/";
		const string toolsPage = "https://devildaggers.info/tools";

		ImGui.SetCursorPos(new(512 - ImGuiUtils.GetTextSize(homePage) / 2, 740));
		if (ImGui.Button(homePage))
			Process.Start(new ProcessStartInfo(homePage) { UseShellExecute = true });

		ImGui.SetCursorPos(new(4, 156));
		if (ImGui.Button(toolsPage))
			Process.Start(new ProcessStartInfo(homePage) { UseShellExecute = true });

		ImGui.PopStyleColor();
		ImGui.PopStyleColor();
		ImGui.PopStyleColor();
		ImGui.PopStyleColor();

		SettingsLayout.Render(ref _showSettings);

		ImGui.End();

		static void TextAt(string text, int x, int y, Vector4 color = default, bool center = false)
		{
			if (center)
			{
				float textSize = ImGuiUtils.GetTextSize(text);
				ImGui.SetCursorPos(new(x - textSize / 2, y));
			}
			else
			{
				ImGui.SetCursorPos(new(x, y));
			}

			if (color == default)
				ImGui.Text(text);
			else
				ImGui.TextColored(color, text);
		}

		static bool MainButtonAt(int x, int y, Vector4 color, string text)
		{
			int xPos = x switch
			{
				0 => 160,
				1 => 416,
				_ => 672,
			};
			int yPos = y * 128 + 256;

			ImGui.PushStyleColor(ImGuiCol.Button, color);
			ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color + new Vector4(0, 0, 0, 0.2f));
			ImGui.PushStyleColor(ImGuiCol.ButtonActive, color + new Vector4(0, 0, 0, 0.3f));

			ImGui.SetCursorPos(new(xPos, yPos));
			bool value = ImGui.Button(text, new(192, 96));

			ImGui.PopStyleColor();
			ImGui.PopStyleColor();
			ImGui.PopStyleColor();

			return value;
		}
	}
}
