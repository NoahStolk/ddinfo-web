using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Spawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class SpawnEntry : AbstractComponent
{
	private readonly SpawnUiEntry _spawnUiEntry;
	private readonly Enemy? _enemy;
	private readonly Color _enemyColor;

	public SpawnEntry(IBounds bounds, SpawnUiEntry spawnUiEntry)
		: base(bounds)
	{
		_spawnUiEntry = spawnUiEntry;
		_enemy = spawnUiEntry.EnemyType.GetEnemy();
		_enemyColor = _enemy == null ? Color.Gray(0.75f) : _enemy.Color.ToWarpColor();

		Index = spawnUiEntry.Index;
	}

	public bool Hover { get; private set; }
	public int Index { get; }

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		Hover = MouseUiContext.Contains(parentPosition, Bounds);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		bool isSelected = StateManager.SpawnEditorState.SelectedIndices.Contains(Index);
		Color background = (isSelected, Hover) switch
		{
			(true, true) => new(0, 127, 255, 127),
			(true, false) => new(0, 127, 255, 63),
			(false, true) => Color.Gray(0.2f),
			_ => Color.Invisible,
		};
		if (background != Color.Invisible)
			RenderBatchCollector.RenderRectangleTopLeft(Bounds.Size, parentPosition + Bounds.TopLeft, Depth, background);

		RenderText(Rectangle.At(Bounds.X1, Bounds.Y1, 96, Spawns.SpawnEntryHeight), _enemyColor, _enemy?.Name ?? "Empty", TextAlign.Left, FontSize.F8X8);
		RenderText(Rectangle.At(Bounds.X1 + 96, Bounds.Y1, 96, Spawns.SpawnEntryHeight), Color.White, _spawnUiEntry.Delay.ToString("0.0000"), TextAlign.Right, FontSize.F8X8);
		RenderText(Rectangle.At(Bounds.X1 + 192, Bounds.Y1, 96, Spawns.SpawnEntryHeight), Color.White, _spawnUiEntry.Seconds.ToString("0.0000"), TextAlign.Right, FontSize.F8X8);
		RenderText(Rectangle.At(Bounds.X1 + 288, Bounds.Y1, 48, Spawns.SpawnEntryHeight), Color.White, NoFarmGemsString(_spawnUiEntry.NoFarmGems), TextAlign.Right, FontSize.F8X8);
		RenderText(Rectangle.At(Bounds.X1 + 336, Bounds.Y1, 48, Spawns.SpawnEntryHeight), GetColorFromHand(_spawnUiEntry.GemState.HandLevel), _spawnUiEntry.GemState.Value.ToString(), TextAlign.Right, FontSize.F8X8);

		void RenderText(IBounds bounds, Color textColor, string text, TextAlign textAlign, FontSize fontSize)
		{
			int padding = (int)MathF.Round((bounds.Y2 - bounds.Y1) / 4f);
			Vector2i<int> textPosition = textAlign switch
			{
				TextAlign.Middle => new Vector2i<int>(bounds.X1 + bounds.X2, bounds.Y1 + bounds.Y2) / 2,
				TextAlign.Left => new(bounds.X1 + padding, bounds.Y1 + padding),
				TextAlign.Right => new(bounds.X2 - padding, bounds.Y1 + padding),
				_ => throw new InvalidOperationException("Invalid text align."),
			};

			RenderBatchCollector.RenderMonoSpaceText(fontSize, Vector2i<int>.One, parentPosition + textPosition, Depth + 2, textColor, text, textAlign);
		}

		Color GetColorFromHand(HandLevel handLevel) => handLevel switch
		{
			HandLevel.Level3 => UpgradeColors.Level3.ToWarpColor(),
			HandLevel.Level4 => UpgradeColors.Level4.ToWarpColor(),
			_ => Color.Red,
		};

		static string NoFarmGemsString(int gems) => gems == 0 ? "-" : $"+{gems}";
	}
}
