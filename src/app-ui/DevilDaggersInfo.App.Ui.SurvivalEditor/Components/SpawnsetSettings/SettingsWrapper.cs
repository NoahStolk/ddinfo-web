using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSettings;

public class SettingsWrapper : AbstractComponent
{
	public SettingsWrapper(Rectangle metric)
		: base(metric)
	{
		const int settingWidth = 112;
		const int settingHeight = 16;

		foreach (Button button in GetHandButtons())
			NestingContext.Add(button);
		AddTextInputSetting("Addit. gems", 0, settingHeight, ChangeAdditionalGems);
		AddTextInputSetting("Timer start", 0, settingHeight * 2, ChangeTimerStart);

		void ChangeAdditionalGems(string input) => SpawnsetSettingEditUtils.ChangeSetting<int>(v => StateManager.SpawnsetState.Spawnset with { AdditionalGems = v }, input, "Changed additional gems");
		void ChangeTimerStart(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { TimerStart = v }, input, "Changed timer start");

		IEnumerable<Button> GetHandButtons()
		{
			return Enum.GetValues<HandLevel>()
				.Select((hl, i) => new Button(Rectangle.At(i * 56, 0, 56, settingHeight), () => UpdateHand(hl), Color.Black, GetColor(hl), Color.Gray(0.25f), Color.White, ToShortString(hl), TextAlign.Middle, 2, FontSize.F8X8))
				.ToList();

			void UpdateHand(HandLevel handLevel)
			{
				StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with
				{
					HandLevel = handLevel,
				});
				SpawnsetHistoryManager.Save($"Set hand level to {handLevel}");
			}

			Color GetColor(HandLevel handLevel) => handLevel switch
			{
				HandLevel.Level1 => ToWarpColor(UpgradeColors.Level1),
				HandLevel.Level2 => ToWarpColor(UpgradeColors.Level2),
				HandLevel.Level3 => ToWarpColor(UpgradeColors.Level3),
				HandLevel.Level4 => ToWarpColor(UpgradeColors.Level4),
				_ => throw new InvalidEnumConversionException(handLevel),
			};

			string ToShortString(HandLevel handLevel) => handLevel switch
			{
				HandLevel.Level1 => "Lvl 1",
				HandLevel.Level2 => "Lvl 2",
				HandLevel.Level3 => "Lvl 3",
				HandLevel.Level4 => "Lvl 4",
				_ => throw new InvalidEnumConversionException(handLevel),
			};

			Color ToWarpColor(DevilDaggersInfo.Core.Wiki.Structs.Color c) => new(c.R, c.G, c.B, 255);
		}

		void AddTextInputSetting(string labelText, int x, int y, Action<string> onChange)
		{
			NestingContext.Add(AddLabel(labelText, x, y));
			NestingContext.Add(ComponentBuilder.CreateTextInput(Rectangle.At(x + settingWidth, y, settingWidth, settingHeight), true, onChange));
		}

		Label AddLabel(string labelText, int x, int y)
		{
			return new(Rectangle.At(x, y, settingWidth, settingHeight), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
		}
	}
}
