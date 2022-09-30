using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Extensions;

public static class SpawnsetEditTypeExtensions
{
	public static string GetChange(this SpawnsetEditType spawnsetEditType) => spawnsetEditType switch
	{
		SpawnsetEditType.Reset => "Spawnset edit",
		SpawnsetEditType.ArenaPencil => "Arena pencil edit",
		SpawnsetEditType.ArenaLine => "Arena line edit",
		SpawnsetEditType.ArenaRectangle => "Arena rectangle edit",
		SpawnsetEditType.ArenaBucket => "Arena bucket edit",
		SpawnsetEditType.ShrinkStart => "Shrink start change",
		SpawnsetEditType.ShrinkEnd => "Shrink end change",
		SpawnsetEditType.ShrinkRate => "Shrink rate change",
		SpawnsetEditType.Brightness => "Brightness change",
		SpawnsetEditType.Format => "Format change",
		SpawnsetEditType.HandLevel => "Hand level change",
		SpawnsetEditType.AdditionalGems => "Additional gems change",
		SpawnsetEditType.TimerStart => "Timer start change",
		_ => throw new InvalidEnumConversionException(spawnsetEditType),
	};
}
