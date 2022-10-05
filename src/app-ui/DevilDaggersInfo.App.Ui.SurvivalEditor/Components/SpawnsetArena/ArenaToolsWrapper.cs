using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaToolsWrapper : AbstractComponent
{
	private const int _arenaButtonSize = 20;

	public ArenaToolsWrapper(Rectangle metric)
		: base(metric)
	{
		AddToolButton(0, 0, ArenaTool.Pencil, "P");
		AddToolButton(0 + _arenaButtonSize, 0, ArenaTool.Line, "L");
		AddToolButton(0 + _arenaButtonSize * 2, 0, ArenaTool.Rectangle, "R");
		AddToolButton(0 + _arenaButtonSize * 3, 0, ArenaTool.Bucket, "B");
		AddToolButton(0, _arenaButtonSize, ArenaTool.Dagger, "D");

		AddBucketButtons(0, 0);
	}

	private void AddToolButton(int offsetX, int offsetY, ArenaTool arenaTool, string text)
	{
		ArenaButton button = new(Rectangle.At(offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.SetArenaTool(arenaTool), Color.Yellow, Color.Black, text, FontSize.F8X8);
		NestingContext.Add(button);
	}

	private void AddBucketButtons(int offsetX, int offsetY)
	{
		void ChangeBucketTolerance(string s) => ParseUtils.TryParseAndExecute<float>(s, 0, StateManager.SetArenaBucketTolerance);
		void ChangeBucketVoidHeight(string s) => ParseUtils.TryParseAndExecute<float>(s, StateManager.SetArenaBucketVoidHeight);

		TextInput bucketTolerance = new(Rectangle.At(offsetX, offsetY + _arenaButtonSize * 2, 80, 16), true, ChangeBucketTolerance, ChangeBucketTolerance, ChangeBucketTolerance, Color.Black, Color.White, Color.Gray(63), Color.White, Color.White, Color.White, Color.Gray(63), 2, FontSize.F8X8, 8, 4);
		TextInput bucketVoidHeight = new(Rectangle.At(offsetX, offsetY + _arenaButtonSize * 3, 80, 16), true, ChangeBucketVoidHeight, ChangeBucketVoidHeight, ChangeBucketVoidHeight, Color.Black, Color.White, Color.Gray(63), Color.White, Color.White, Color.White, Color.Gray(63), 2, FontSize.F8X8, 8, 4);

		bucketTolerance.SetText(StateManager.ArenaEditorState.BucketTolerance.ToString("0.0"));
		bucketVoidHeight.SetText(StateManager.ArenaEditorState.BucketVoidHeight.ToString("0.0"));

		NestingContext.Add(bucketTolerance);
		NestingContext.Add(bucketVoidHeight);
	}
}
