using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui;

public static class UpdateWindow
{
	public static List<string> LogMessages { get; } = new();

	public static void Render(ref bool show)
	{
		if (!show)
			return;

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		ImGui.SetNextWindowPos(center, ImGuiCond.Always, new(0.5f, 0.5f));
		ImGui.SetNextWindowSize(new(384, 384));
		if (ImGui.Begin("Update available", ref show, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
		{
			if (ImGui.Button("Update"))
				Task.Run(async () => await UpdateLogic.RunAsync());

			LogMessages.ForEach(ImGui.Text);
		}

		ImGui.End();
	}
}
