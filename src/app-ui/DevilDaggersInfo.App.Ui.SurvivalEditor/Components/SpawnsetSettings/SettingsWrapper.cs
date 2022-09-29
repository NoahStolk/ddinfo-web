using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSettings;

public class SettingsWrapper : AbstractComponent
{
	private readonly Button _buttonLevel1;
	private readonly Button _buttonLevel2;
	private readonly Button _buttonLevel3;
	private readonly Button _buttonLevel4;

	public SettingsWrapper(Rectangle metric)
		: base(metric)
	{
		const int settingWidth = 112;
		const int settingHeight = 16;

		_buttonLevel1 = CreateHandButton(HandLevel.Level1);
		_buttonLevel2 = CreateHandButton(HandLevel.Level2);
		_buttonLevel3 = CreateHandButton(HandLevel.Level3);
		_buttonLevel4 = CreateHandButton(HandLevel.Level4);

		NestingContext.Add(_buttonLevel1);
		NestingContext.Add(_buttonLevel2);
		NestingContext.Add(_buttonLevel3);
		NestingContext.Add(_buttonLevel4);

		AddTextInputSetting("Addit. gems", 0, settingHeight, ChangeAdditionalGems);
		AddTextInputSetting("Timer start", 0, settingHeight * 2, ChangeTimerStart);

		void ChangeAdditionalGems(string input) => SpawnsetSettingEditUtils.ChangeSetting<int>(v => StateManager.SpawnsetState.Spawnset with { AdditionalGems = v }, input, "Changed additional gems");
		void ChangeTimerStart(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { TimerStart = v }, input, "Changed timer start");

		Button CreateHandButton(HandLevel handLevel)
		{
			int i = (int)handLevel - 1;
			return new(Rectangle.At(i * 56, 0, 56, settingHeight), UpdateHand, Color.Black, GetColor(handLevel), Color.Gray(0.25f), Color.White, ToShortString(), TextAlign.Middle, 2, FontSize.F8X8);

			void UpdateHand()
			{
				StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with
				{
					HandLevel = handLevel,
				});
				SpawnsetHistoryManager.Save($"Set hand level to {handLevel}");
			}

			string ToShortString() => handLevel switch
			{
				HandLevel.Level1 => "Lvl 1",
				HandLevel.Level2 => "Lvl 2",
				HandLevel.Level3 => "Lvl 3",
				HandLevel.Level4 => "Lvl 4",
				_ => throw new InvalidEnumConversionException(handLevel),
			};
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

	public void SetSpawnset()
	{
		_buttonLevel1.BackgroundColor = GetBackground(HandLevel.Level1);
		_buttonLevel2.BackgroundColor = GetBackground(HandLevel.Level2);
		_buttonLevel3.BackgroundColor = GetBackground(HandLevel.Level3);
		_buttonLevel4.BackgroundColor = GetBackground(HandLevel.Level4);

		Color GetBackground(HandLevel handLevel)
		{
			if (StateManager.SpawnsetState.Spawnset.SpawnVersion < 5)
				return Color.Gray(0.5f);

			return handLevel == StateManager.SpawnsetState.Spawnset.HandLevel ? GetColor(handLevel) : Color.Black;
		}
	}

	private static Color GetColor(HandLevel handLevel) => handLevel switch
	{
		HandLevel.Level1 => UpgradeColors.Level1.ToWarpColor(),
		HandLevel.Level2 => UpgradeColors.Level2.ToWarpColor(),
		HandLevel.Level3 => UpgradeColors.Level3.ToWarpColor(),
		HandLevel.Level4 => UpgradeColors.Level4.ToWarpColor(),
		_ => throw new InvalidEnumConversionException(handLevel),
	};
}
