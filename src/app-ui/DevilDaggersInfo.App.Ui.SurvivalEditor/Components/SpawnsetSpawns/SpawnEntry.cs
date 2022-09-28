using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class SpawnEntry : AbstractComponent
{
	public SpawnEntry(Rectangle metric, EditableSpawn spawn)
		: base(metric)
	{
		Enemy? enemy = spawn.EnemyType switch
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
		Color backgroundColor = enemy == null ? Color.Black : new(enemy.Color.R, enemy.Color.G, enemy.Color.B, byte.MaxValue);
		Color textColor = backgroundColor.ReadableColorForBrightness();

		const byte hoverComponent = 128;
		Color hoverAddition = new(hoverComponent, hoverComponent, hoverComponent, 255);
		NestingContext.Add(new Button(Rectangle.At(0, 0, 96, Spawns.SpawnEntryHeight), () => {}, backgroundColor, Color.Black, backgroundColor.Intensify(hoverComponent), textColor, enemy?.Name ?? "Empty", TextAlign.Left, 2, FontSize.F8X8));
		NestingContext.Add(new Button(Rectangle.At(96, 0, 96, Spawns.SpawnEntryHeight), () => {}, Color.Transparent, Color.Black, hoverAddition, Color.White, spawn.Delay.ToString("0.0000"), TextAlign.Right, 2, FontSize.F8X8));
		NestingContext.Add(new Button(Rectangle.At(192, 0, 96, Spawns.SpawnEntryHeight), () => {}, Color.Transparent, Color.Black, hoverAddition, Color.White, spawn.Seconds.ToString("0.0000"), TextAlign.Right, 2, FontSize.F8X8));
		NestingContext.Add(new Label(Rectangle.At(288, 0, 48, Spawns.SpawnEntryHeight), Color.White, spawn.NoFarmGems.ToString(), TextAlign.Right, FontSize.F8X8));
		NestingContext.Add(new Label(Rectangle.At(336, 0, 48, Spawns.SpawnEntryHeight), GetColorFromHand(spawn.GemState.HandLevel), spawn.GemState.Value.ToString(), TextAlign.Right, FontSize.F8X8));

		Color GetColorFromHand(HandLevel handLevel) => handLevel switch
		{
			HandLevel.Level3 => Color.Aqua,
			HandLevel.Level4 => Color.Purple,
			_ => Color.Red,
		};
	}
}
