using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Types.Core.Spawnsets;
using System.Collections.Immutable;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class SpawnEditor : AbstractComponent
{
	private readonly Dictionary<EnemyType, Button> _enemyTypeButtons = new();

	// TODO: On spawn select, set these values.
	private EnemyType _selectedEnemyType;
	private float _selectedDelay;

	public SpawnEditor(Rectangle metric)
		: base(metric)
	{
		const int width = 96;

		AddEnemyButton(96, 0, EnemyType.Empty);
		AddEnemyButton(144, 0, EnemyType.Squid1);
		AddEnemyButton(192, 0, EnemyType.Squid2);
		AddEnemyButton(240, 0, EnemyType.Squid3);
		AddEnemyButton(288, 0, EnemyType.Leviathan);
		AddEnemyButton(336, 0, EnemyType.Thorn);
		AddEnemyButton(144, 16, EnemyType.Spider1);
		AddEnemyButton(192, 16, EnemyType.Spider2);
		AddEnemyButton(240, 16, EnemyType.Centipede);
		AddEnemyButton(288, 16, EnemyType.Gigapede);
		AddEnemyButton(336, 16, EnemyType.Ghostpede);

		Label enemyTypeLabel = new(Rectangle.At(0, 0, width, 16), Color.White, "Enemy", TextAlign.Left, FontSize.F8X8);
		Label delayLabel = new(Rectangle.At(0, 32, width, 16), Color.White, "Delay", TextAlign.Left, FontSize.F8X8);
		TextInput delayTextInput = ComponentBuilder.CreateTextInput(Rectangle.At(288, 32, width, 16), true, OnDelayChange, OnDelayChange, OnDelayChange);
		delayTextInput.SetText("0.0000");

		NestingContext.Add(enemyTypeLabel);
		NestingContext.Add(delayLabel);
		NestingContext.Add(delayTextInput);

		Button addButton = new(Rectangle.At(96, 80, width, 32), AddSpawn, new(0, 127, 0, 255), Color.White, new(0, 191, 0, 255), Color.White, "ADD", TextAlign.Middle, 2, FontSize.F12X12);
		Button insertButton = new(Rectangle.At(192, 80, width, 32), InsertSpawn, new(0, 63, 127, 255), Color.White, new(0, 95, 191, 255), Color.White, "INSERT", TextAlign.Middle, 2, FontSize.F12X12);
		Button editButton = new(Rectangle.At(288, 80, width, 32), EditSpawn, new(127, 63, 0, 255), Color.White, new(191, 95, 0, 255), Color.White, "EDIT", TextAlign.Middle, 2, FontSize.F12X12);

		NestingContext.Add(addButton);
		NestingContext.Add(insertButton);
		NestingContext.Add(editButton);

		void AddEnemyButton(int x, int y, EnemyType enemyType)
		{
			Enemy? enemy = enemyType.GetEnemy();
			Color enemyColor = enemy?.Color.ToWarpColor() ?? Color.Gray(0.75f);
			string enemyName = enemyType.GetShortName();
			Color borderColor = _selectedEnemyType == enemyType ? Color.White : Color.Black;
			Color hoverBackgroundColor = Color.Lerp(enemyColor, Color.White, 0.5f);
			Button button = new(Rectangle.At(x, y, 48, 16), () => SetSelectedEnemyType(enemyType), enemyColor, borderColor, hoverBackgroundColor, enemyColor.ReadableColorForBrightness(), enemyName, TextAlign.Left, 2, FontSize.F8X8);
			_enemyTypeButtons.Add(enemyType, button);
			NestingContext.Add(button);
		}
	}

	private void OnDelayChange(string s) => ParseUtils.TryParseAndExecute<float>(s, 0, f => _selectedDelay = f);

	private void SetSelectedEnemyType(EnemyType enemyType)
	{
		_selectedEnemyType = enemyType;

		foreach (KeyValuePair<EnemyType, Button> kvp in _enemyTypeButtons)
			kvp.Value.BorderColor = _selectedEnemyType == kvp.Key ? Color.White : Color.Black;
	}

	private void AddSpawn()
	{
		List<Spawn> newSpawns = StateManager.SpawnsetState.Spawnset.Spawns.ToList();
		newSpawns.Add(new(_selectedEnemyType, _selectedDelay));

		// TODO: Scroll down: SpawnsWrapper.SetScrollPercentage(1);
		StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with { Spawns = newSpawns.ToImmutableArray() });
		SpawnsetHistoryManager.Save(SpawnsetEditType.SpawnAdd);
	}

	private void EditSpawn()
	{
		ImmutableArray<Spawn> newSpawns = StateManager.SpawnsetState.Spawnset.Spawns.Select((t, i) => StateManager.SpawnEditorState.SelectedIndices.Contains(i) ? new(_selectedEnemyType, _selectedDelay) : t).ToImmutableArray();

		StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with { Spawns = newSpawns });
		SpawnsetHistoryManager.Save(SpawnsetEditType.SpawnEdit);
	}

	private void InsertSpawn()
	{
		int firstSelection = StateManager.SpawnEditorState.SelectedIndices.Count == 0 ? 0 : StateManager.SpawnEditorState.SelectedIndices.Min();
		ImmutableArray<Spawn> spawns = StateManager.SpawnsetState.Spawnset.Spawns.Insert(firstSelection, new(_selectedEnemyType, _selectedDelay));

		int[] indices = StateManager.SpawnEditorState.SelectedIndices.ToArray();
		StateManager.ClearSpawnSelections();
		foreach (int index in indices)
			StateManager.SelectSpawn(index + 1);

		StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with { Spawns = spawns });
		SpawnsetHistoryManager.Save(SpawnsetEditType.SpawnAdd);
	}
}
