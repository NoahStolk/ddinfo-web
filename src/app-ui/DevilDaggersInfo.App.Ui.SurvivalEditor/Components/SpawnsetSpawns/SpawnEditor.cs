using DevilDaggersInfo.App.Ui.Base;
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
	private readonly Dictionary<EnemyType, Button> _enemyTypeButtons = new();

	private EnemyType _selectedEnemyType;
	private float _selectedDelay;

	public SpawnEditor(Rectangle metric)
		: base(metric)
	{
		const int width = 80;
		const int height = 16;

		AddEnemyButton(0, 0, EnemyType.Empty, Color.Gray(0.75f), "Empty");

		for (int i = 0; i < 10; i++)
		{
			EnemyType enemyType = (EnemyType)i;
			Enemy? enemy = enemyType.GetEnemy();
			if (enemy != null)
				AddEnemyButton(i % 2 * width, height + i / 2 * height, enemyType, enemy.Color.ToWarpColor(), enemy.Name);
		}

		void OnDelayChange(string s)
		{
			ParseUtils.TryParseAndExecute<float>(s, 0, f => _selectedDelay = f);
		}

		TextInput delayTextInput = ComponentBuilder.CreateTextInput(Rectangle.At(width * 2, 0, 64, 16), true, OnDelayChange, OnDelayChange, OnDelayChange);
		NestingContext.Add(delayTextInput);

		void AddEnemyButton(int x, int y, EnemyType enemyType, Color enemyColor, string enemyName)
		{
			Color borderColor = _selectedEnemyType == enemyType ? Color.White : Color.Black;
			Button button = new(Rectangle.At(x, y, width, height), () => SetSelectedEnemyType(enemyType), enemyColor, borderColor, Color.Invert(enemyColor), enemyColor.ReadableColorForBrightness(), enemyName, TextAlign.Left, 2, FontSize.F8X8);
			_enemyTypeButtons.Add(enemyType, button);
			NestingContext.Add(button);
		}
	}

	private void SetSelectedEnemyType(EnemyType enemyType)
	{
		_selectedEnemyType = enemyType;

		foreach (KeyValuePair<EnemyType, Button> kvp in _enemyTypeButtons)
			kvp.Value.BorderColor = _selectedEnemyType == kvp.Key ? Color.White : Color.Black;
	}
}
