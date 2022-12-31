using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;

namespace DevilDaggersInfo.App.Ui.Base.Styling;

public static class GlobalStyles
{
	static GlobalStyles()
	{
		foreach (EnemyType enemyType in Enum.GetValues<EnemyType>())
		{
			Enemy? enemy = enemyType.GetEnemy();
			Color enemyColor = enemy?.Color.ToWarpColor() ?? Color.Gray(0.75f);
			Color hoverBackgroundColor = Color.Lerp(enemyColor, Color.White, 0.5f);

			EnemyButtonStyles[enemyType] = new(enemyColor, Color.Black, hoverBackgroundColor, 1);
			SelectedEnemyButtonStyles[enemyType] = new(enemyColor, Color.White, hoverBackgroundColor, 1);
		}

		foreach (HandLevel handLevel in Enum.GetValues<HandLevel>())
		{
			Color handColor = handLevel.GetColor();
			HandLevelButtonStyles[handLevel] = new(Color.Black, handColor, Color.Gray(0.6f), 1);
			SelectedHandLevelButtonStyles[handLevel] = new(handColor, handColor, Color.Gray(0.6f), 1);
		}
	}

	public static ScrollAreaStyle DefaultScrollAreaStyle { get; } = new(2, 2, Color.White, Color.Gray(0.75f), Color.Black, Color.Gray(0.5f), Color.Gray(0.25f));

	public static LabelStyle PopupLabel { get; } = new(Color.White, TextAlign.Middle, FontSize.H16);

	public static LabelStyle LabelDefaultLeft { get; } = new(Color.White, TextAlign.Left, FontSize.H12);
	public static LabelStyle LabelDefaultMiddle { get; } = new(Color.White, TextAlign.Middle, FontSize.H12);
	public static LabelStyle LabelDefaultRight { get; } = new(Color.White, TextAlign.Right, FontSize.H12);

	public static LabelStyle LabelTitle { get; } = new(Color.White, TextAlign.Middle, FontSize.H24);

	public static SliderStyle DefaultSliderStyle { get; } = new(Color.White, 2, TextAlign.Middle, FontSize.H12);

	public static DropdownStyle DefaultDropdownStyle { get; } = new(FontSize.H12);

	public static DropdownEntryStyle DefaultDropdownEntryStyle { get; } = new(TextAlign.Left, FontSize.H12);

	public static TextInputStyle TextInput { get; } = new(Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 2, 4, FontSize.H12);
	public static TextInputStyle SpawnsetTextInput { get; } = new(Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 2, 8, FontSize.H12);

	public static ButtonStyle Borderless { get; } = new(Color.Invisible, Color.Invisible, Color.Invisible, 0);
	public static ButtonStyle DefaultButtonStyle { get; } = new(Color.Black, Color.White, Color.Gray(0.5f), 1);
	public static ButtonStyle NavigationButtonStyle { get; } = new(Color.Black, Color.Gray(0.75f), Color.Gray(0.5f), 1);
	public static ButtonStyle ActiveToolButtonStyle { get; } = new(Color.Blue, Color.White, Color.Blue, 1);

	public static TextButtonStyle DefaultLeft { get; } = new(Color.White, TextAlign.Left, FontSize.H12);
	public static TextButtonStyle DefaultMiddle { get; } = new(Color.White, TextAlign.Middle, FontSize.H12);
	public static TextButtonStyle Popup { get; } = new(Color.White, TextAlign.Middle, FontSize.H16);
	public static TextButtonStyle ConfigButton { get; } = new(Color.White, TextAlign.Middle, FontSize.H12);

	public static TextButtonStyle PathDirectoryButton { get; } = new(Color.Yellow, TextAlign.Left, FontSize.H12);
	public static TextButtonStyle PathFileButton { get; } = new(Color.White, TextAlign.Left, FontSize.H12);
	public static TextButtonStyle FileSaveButton { get; } = new(Color.Green, TextAlign.Middle, FontSize.H12);

	public static ButtonStyle AddSpawn { get; } = new(new(0, 127, 0, 255), Color.White, new(0, 191, 0, 255), 1);
	public static ButtonStyle InsertSpawn { get; } = new(new(0, 63, 127, 255), Color.White, new(0, 95, 191, 255), 1);
	public static ButtonStyle EditSpawn { get; } = new(new(127, 63, 0, 255), Color.White, new(191, 95, 0, 255), 1);
	public static TextButtonStyle SpawnText { get; } = new(Color.White, TextAlign.Middle, FontSize.H16);

	public static ButtonStyle SpawnsetSetting { get; } = new(Color.Black, Color.Gray(0.5f), Color.Gray(0.5f), 1);
	public static ButtonStyle SelectedSpawnsetSetting { get; } = new(Color.Gray(0.5f), Color.White, Color.Gray(0.75f), 1);
	public static TextButtonStyle View3dButton { get; } = new(Color.White, TextAlign.Middle, FontSize.H12);
	public static TextButtonStyle SelectedHeightButton { get; } = new(Color.Blue, TextAlign.Middle, FontSize.H12);

	public static TextButtonStyle HandLevelText { get; } = new(Color.White, TextAlign.Middle, FontSize.H12);

	public static Dictionary<EnemyType, ButtonStyle> EnemyButtonStyles { get; } = new();
	public static Dictionary<EnemyType, ButtonStyle> SelectedEnemyButtonStyles { get; } = new();

	public static Dictionary<HandLevel, ButtonStyle> HandLevelButtonStyles { get; } = new();
	public static Dictionary<HandLevel, ButtonStyle> SelectedHandLevelButtonStyles { get; } = new();
}
