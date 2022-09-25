using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaWrapper : AbstractComponent
{
	private const int _arenaButtonSize = 24;

	public ArenaWrapper(Rectangle metric)
		: base(metric)
	{
		Arena arena = new(default, 6);
		NestingContext.Add(arena);

		int buttonsOffsetX = arena.Metric.Size.X + 8;

		int offset = 0;
		foreach (float height in new[] { -1000, -1.1f, -1.01f, -1, -0.8f, -0.6f, -0.4f, -0.2f })
			AddHeightButton(height, offset++ * _arenaButtonSize, 0);

		for (int i = 0; i < 64; i++)
		{
			int offsetX = i % 8 * _arenaButtonSize;
			int offsetY = i / 8 * _arenaButtonSize;
			AddHeightButton(i, offsetX, offsetY + 36);
		}

		const int toolButtonOffsetY = 240;
		AddToolButton(0, toolButtonOffsetY, ArenaTool.Pencil, "P");
		AddToolButton(24, toolButtonOffsetY, ArenaTool.Line, "L");
		AddToolButton(48, toolButtonOffsetY, ArenaTool.Rectangle, "R");
		AddToolButton(72, toolButtonOffsetY, ArenaTool.Bucket, "B");

		void AddSetting(string labelText, int y1)
		{
			const int labelWidth = 112;
			Label label = new(Rectangle.At(0, y1, labelWidth, 16), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
			TextInput textInput = ComponentBuilder.CreateTextInput(Rectangle.At(labelWidth, y1, 64, 16), true);
			NestingContext.Add(label);
			NestingContext.Add(textInput);
		}

		AddSetting("Shrink start", arena.Metric.Size.Y + 8);
		AddSetting("Shrink end", arena.Metric.Size.Y + 24);
		AddSetting("Shrink rate", arena.Metric.Size.Y + 40);
		AddSetting("Brightness", arena.Metric.Size.Y + 56);

		void AddHeightButton(float height, int offsetX, int offsetY)
		{
			HeightButton button = new(Rectangle.At(buttonsOffsetX + offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.SetArenaSelectedHeight(height), height);
			NestingContext.Add(button);
		}

		void AddToolButton(int offsetX, int offsetY, ArenaTool arenaTool, string text)
		{
			ArenaButton button = new(Rectangle.At(buttonsOffsetX + offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.SetArenaTool(arenaTool), Color.Yellow, Color.Black, text, FontSize.F8X8);
			NestingContext.Add(button);
		}
	}
}
