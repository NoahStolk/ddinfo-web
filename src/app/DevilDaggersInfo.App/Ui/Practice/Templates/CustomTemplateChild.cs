using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Extensions;
using DevilDaggersInfo.App.User.Settings;
using DevilDaggersInfo.App.User.Settings.Model;
using DevilDaggersInfo.Common;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Practice.Templates;

public static class CustomTemplateChild
{
	public static void Render(
		UserSettingsModel.UserSettingsPracticeTemplate customTemplate,
		bool isActive,
		Vector2 buttonSize,
		Action onClick)
	{
		string buttonName = $"{customTemplate.HandLevel}-{customTemplate.AdditionalGems}-{customTemplate.TimerStart.ToString(StringFormats.TimeFormat)}";

		Color color = Color.White;

		(string gemsOrHomingText, Color gemColor) = PracticeWindow.GetGemsOrHomingText(customTemplate.HandLevel, customTemplate.AdditionalGems);
		(byte backgroundAlpha, byte textAlpha) = PracticeWindow.GetAlpha(isActive);

		ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
		if (ImGui.BeginChild(buttonName, buttonSize, true))
		{
			bool hover = ImGui.IsWindowHovered();
			ImGui.PushStyleColor(ImGuiCol.ChildBg, hover ? color with { A = 31 } : color with { A = backgroundAlpha });

			if (ImGui.BeginChild(buttonName + " child", buttonSize, false, ImGuiWindowFlags.NoInputs))
			{
				if (hover && ImGui.IsMouseReleased(ImGuiMouseButton.Left))
					onClick.Invoke();

				float windowWidth = ImGui.GetWindowWidth();

				ImGui.SetCursorPos(ImGui.GetCursorPos() + new Vector2(8, 8));

				string timerStartString = customTemplate.TimerStart.ToString(StringFormats.TimeFormat);

				if (isActive)
				{
					float width = buttonSize.X - 96;
					if (ImGui.BeginChild(buttonName + " name", new(width, 13), false))
					{
						string name = customTemplate.Name ?? string.Empty;

						ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, Vector2.Zero);
						ImGui.PushItemWidth(width);
						if (ImGui.InputText("##name", ref name, 32))
						{
							UserSettingsModel.UserSettingsPracticeTemplate originalTemplate = UserSettings.Model.PracticeTemplates.First(pt => pt == customTemplate);
							int index = UserSettings.Model.PracticeTemplates.IndexOf(originalTemplate);

							List<UserSettingsModel.UserSettingsPracticeTemplate> newList = UserSettings.Model.PracticeTemplates
								.Where(pt => pt != originalTemplate)
								.ToList();

							newList.Insert(index, originalTemplate with
							{
								Name = name,
							});

							UserSettings.Model = UserSettings.Model with
							{
								PracticeTemplates = newList,
							};
						}

						ImGui.PopItemWidth();
						ImGui.PopStyleVar();
					}

					ImGui.EndChild();
				}
				else
				{
					ImGui.TextColored(color with { A = textAlpha }, string.IsNullOrWhiteSpace(customTemplate.Name) ? "<untitled>" : customTemplate.Name);
				}

				ImGui.SameLine(windowWidth - ImGui.CalcTextSize(timerStartString).X - 8);
				ImGui.TextColored(Color.White with { A = textAlpha }, timerStartString);

				ImGui.SetCursorPos(ImGui.GetCursorPos() + new Vector2(8, 0));

				ImGui.TextColored(customTemplate.HandLevel.GetColor() with { A = textAlpha }, customTemplate.HandLevel.ToString());
				ImGui.SameLine(windowWidth - ImGui.CalcTextSize(gemsOrHomingText).X - 8);
				ImGui.TextColored(gemColor with { A = textAlpha }, gemsOrHomingText);
			}

			ImGui.EndChild();

			ImGui.PopStyleColor();
		}

		ImGui.PopStyleVar();

		ImGui.EndChild();
	}
}
