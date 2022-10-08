using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Extensions;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class SpawnEditor : AbstractComponent
{
	public SpawnEditor(Rectangle metric)
		: base(metric)
	{
		const int width = 80;
		const int height = 16;

		int i = 0;
		foreach (EnemyType enemyType in Enum.GetValues<EnemyType>())
		{
			Enemy? enemy = enemyType.GetEnemy();
			Color enemyColor = enemy?.Color.ToWarpColor() ?? Color.Gray(0.75f);
			Button button = new(Rectangle.At(i % 3 * width, i / 3 * height, width, height), () => {}, enemyColor, Color.White, Color.Invert(enemyColor), enemyColor.ReadableColorForBrightness(), enemy?.Name ?? "Empty", TextAlign.Left, 2, FontSize.F8X8);
			NestingContext.Add(button);

			i++;
		}
	}
}
