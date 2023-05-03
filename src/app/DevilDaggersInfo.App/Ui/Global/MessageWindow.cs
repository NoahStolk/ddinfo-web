using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Global;

public static class MessageWindow
{
	public static void Render(ref bool show, Message message)
	{
		if (!show)
			return;

		ImGui.Begin(message.Title, ref show, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);
		ImGui.Text(message.Text);
		ImGui.End();
	}

	public record Message(string Title, string Text);
}
