using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Spawns;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class SpawnEntry : AbstractComponent
{
	public SpawnEntry(Rectangle metric, SpawnUiEntry spawnUiEntry)
		: base(metric)
	{
		Index = spawnUiEntry.Index;

		Enemy? enemy = spawnUiEntry.EnemyType switch
		{
			EnemyType.Squid1 => EnemiesV3_2.Squid1,
			EnemyType.Squid2 => EnemiesV3_2.Squid2,
			EnemyType.Centipede => EnemiesV3_2.Centipede,
			EnemyType.Spider1 => EnemiesV3_2.Spider1,
			EnemyType.Leviathan => EnemiesV3_2.Leviathan,
			EnemyType.Gigapede => EnemiesV3_2.Gigapede,
			EnemyType.Squid3 => EnemiesV3_2.Squid3,
			EnemyType.Thorn => EnemiesV3_2.Thorn,
			EnemyType.Spider2 => EnemiesV3_2.Spider2,
			EnemyType.Ghostpede => EnemiesV3_2.Ghostpede,
			_ => null,
		};
		Color backgroundColor = enemy == null ? Color.Gray(0.75f) : new(enemy.Color.R, enemy.Color.G, enemy.Color.B, byte.MaxValue);

		NestingContext.Add(new Label(Rectangle.At(0, 0, 96, Spawns.SpawnEntryHeight), backgroundColor, enemy?.Name ?? "Empty", TextAlign.Left, FontSize.F8X8));
		NestingContext.Add(new Label(Rectangle.At(96, 0, 96, Spawns.SpawnEntryHeight), Color.White, spawnUiEntry.Delay.ToString("0.0000"), TextAlign.Right, FontSize.F8X8));
		NestingContext.Add(new Label(Rectangle.At(192, 0, 96, Spawns.SpawnEntryHeight), Color.White, spawnUiEntry.Seconds.ToString("0.0000"), TextAlign.Right, FontSize.F8X8));
		NestingContext.Add(new Label(Rectangle.At(288, 0, 48, Spawns.SpawnEntryHeight), Color.White, NoFarmGemsString(spawnUiEntry.NoFarmGems), TextAlign.Right, FontSize.F8X8));
		NestingContext.Add(new Label(Rectangle.At(336, 0, 48, Spawns.SpawnEntryHeight), GetColorFromHand(spawnUiEntry.GemState.HandLevel), spawnUiEntry.GemState.Value.ToString(), TextAlign.Right, FontSize.F8X8));

		Color GetColorFromHand(HandLevel handLevel) => handLevel switch
		{
			HandLevel.Level3 => UpgradeColors.Level3.ToWarpColor(),
			HandLevel.Level4 => UpgradeColors.Level4.ToWarpColor(),
			_ => Color.Red,
		};

		static string NoFarmGemsString(int gems) => gems == 0 ? "-" : $"+{gems}";
	}

	public bool IsSelected { get; set; }
	public bool Hover { get; private set; }
	public int Index { get; }

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		Hover = MouseUiContext.Contains(parentPosition, Metric);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Color background = IsSelected ? Color.Gray(0.5f) : Hover ? Color.Gray(0.25f) : Color.Transparent;
		if (background != Color.Transparent)
			RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, parentPosition + Metric.TopLeft, Depth, background);
	}
}
