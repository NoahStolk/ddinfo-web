using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
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
	private const int _offset = 16;

	private readonly int _width;
	private readonly int _halfWidth;
	private readonly int _thirdWidth;
	private readonly int _quarterWidth;

	private static readonly Color _selected = Color.Gray(0.4f);
	private static readonly Color _hover = Color.Gray(0.6f);

	private readonly Button _buttonV0V1;
	private readonly Button _buttonV2V3;
	private readonly Button _buttonV3Next;

	private readonly Button _buttonSurvival;
	private readonly Button _buttonTimeAttack;
	private readonly Button _buttonRace;

	private readonly Button _buttonLevel1;
	private readonly Button _buttonLevel2;
	private readonly Button _buttonLevel3;
	private readonly Button _buttonLevel4;

	private readonly Label _labelAdditionalGems;
	private readonly Label _labelTimerStart;
	private readonly SpawnsetTextInput _textInputShrinkStart;
	private readonly SpawnsetTextInput _textInputShrinkEnd;
	private readonly SpawnsetTextInput _textInputShrinkRate;
	private readonly SpawnsetTextInput _textInputBrightness;
	private readonly SpawnsetTextInput _textInputAdditionalGems;
	private readonly SpawnsetTextInput _textInputTimerStart;

	public SettingsWrapper(Rectangle metric)
		: base(metric)
	{
		_width = metric.Size.X;
		_halfWidth = _width / 2;
		_thirdWidth = (int)MathF.Ceiling(_width / 3f);
		_quarterWidth = _halfWidth / 2;

		Label title = new(Rectangle.At(0, 0, _width, 48), Color.White, "Settings", TextAlign.Middle, FontSize.F12X12);
		NestingContext.Add(title);

		int y = title.Metric.Size.Y;
		_buttonV0V1 = CreateFormatButton(y, 0, 8, 4);
		_buttonV2V3 = CreateFormatButton(y, 1, 9, 4);
		_buttonV3Next = CreateFormatButton(y, 2, 9, 6);

		y += _offset;
		_buttonSurvival = CreateGameModeButton(y, GameMode.Survival);
		_buttonTimeAttack = CreateGameModeButton(y, GameMode.TimeAttack);
		_buttonRace = CreateGameModeButton(y, GameMode.Race);

		y += _offset;
		_buttonLevel1 = CreateHandButton(y, HandLevel.Level1);
		_buttonLevel2 = CreateHandButton(y, HandLevel.Level2);
		_buttonLevel3 = CreateHandButton(y, HandLevel.Level3);
		_buttonLevel4 = CreateHandButton(y, HandLevel.Level4);

		y += _offset;
		(_textInputShrinkStart, _) = AddSetting("Shrink start", SpawnsetEditType.ShrinkStart, ref y, ChangeShrinkStart);
		(_textInputShrinkEnd, _) = AddSetting("Shrink end", SpawnsetEditType.ShrinkEnd, ref y, ChangeShrinkEnd);
		(_textInputShrinkRate, _) = AddSetting("Shrink rate", SpawnsetEditType.ShrinkRate, ref y, ChangeShrinkRate);
		(_textInputBrightness, _) = AddSetting("Brightness", SpawnsetEditType.Brightness, ref y, ChangeBrightness);
		(_textInputAdditionalGems, _labelAdditionalGems) = AddSetting("Addit. gems", SpawnsetEditType.AdditionalGems, ref y, ChangeAdditionalGems);
		(_textInputTimerStart, _labelTimerStart) = AddSetting("Timer start", SpawnsetEditType.TimerStart, ref y, ChangeTimerStart);

		NestingContext.Add(_buttonV0V1);
		NestingContext.Add(_buttonV2V3);
		NestingContext.Add(_buttonV3Next);

		NestingContext.Add(_buttonSurvival);
		NestingContext.Add(_buttonTimeAttack);
		NestingContext.Add(_buttonRace);

		NestingContext.Add(_buttonLevel1);
		NestingContext.Add(_buttonLevel2);
		NestingContext.Add(_buttonLevel3);
		NestingContext.Add(_buttonLevel4);
	}

	private TextButton CreateFormatButton(int y, int index, int worldVersion, int spawnVersion)
	{
		string str = SpawnsetBinary.GetGameVersionString(worldVersion, spawnVersion);
		return new(Rectangle.At(index * _thirdWidth, y, _thirdWidth, _offset), UpdateFormat, Color.Black, Color.White, _hover, Color.White, str, TextAlign.Middle, 1, FontSize.F8X8);

		void UpdateFormat()
		{
			StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with { WorldVersion = worldVersion, SpawnVersion = spawnVersion });
			SpawnsetHistoryManager.Save(SpawnsetEditType.Format);
		}
	}

	private TextButton CreateGameModeButton(int y, GameMode gameMode)
	{
		int index = (int)gameMode;
		return new(Rectangle.At(index * _thirdWidth, y, _thirdWidth, _offset), UpdateFormat, Color.Black, Color.White, _hover, Color.White, ToShortString(), TextAlign.Middle, 1, FontSize.F8X8);

		void UpdateFormat()
		{
			StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with { GameMode = gameMode });
			SpawnsetHistoryManager.Save(SpawnsetEditType.GameMode);
		}

		string ToShortString() => gameMode switch
		{
			GameMode.Survival => nameof(GameMode.Survival),
			GameMode.TimeAttack => "TA",
			GameMode.Race => nameof(GameMode.Race),
			_ => throw new InvalidEnumConversionException(gameMode),
		};
	}

	private TextButton CreateHandButton(int y, HandLevel handLevel)
	{
		int index = (int)handLevel - 1;
		return new(Rectangle.At(index * _quarterWidth, y, _quarterWidth, _offset), UpdateHand, Color.Black, GetColor(handLevel), _hover, Color.White, ToShortString(), TextAlign.Middle, 1, FontSize.F8X8);

		void UpdateHand()
		{
			StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with { HandLevel = handLevel });
			SpawnsetHistoryManager.Save(SpawnsetEditType.HandLevel);
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

	private (SpawnsetTextInput TextInput, Label Label) AddSetting(string labelText, SpawnsetEditType spawnsetEditType, ref int y, Action<string> onInput)
	{
		Label label = new(Rectangle.At(0, y, _halfWidth, _offset), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
		SpawnsetTextInput textInput = SpawnsetComponentBuilder.CreateSpawnsetTextInput(Rectangle.At(_halfWidth, y, _halfWidth, _offset), onInput, spawnsetEditType);
		NestingContext.Add(label);
		NestingContext.Add(textInput);
		y += _offset;

		return (textInput, label);
	}

	private static void ChangeAdditionalGems(string input) => SpawnsetSettingEditUtils.ChangeSetting<int>(v => StateManager.SpawnsetState.Spawnset with { AdditionalGems = v }, input);

	private static void ChangeTimerStart(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { TimerStart = v }, input);

	private static void ChangeShrinkStart(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkStart = v }, input);

	private static void ChangeShrinkEnd(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkEnd = v }, input);

	private static void ChangeShrinkRate(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkRate = v }, input);

	private static void ChangeBrightness(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { Brightness = v }, input);

	public void SetSpawnset()
	{
		SpawnsetBinary spawnset = StateManager.SpawnsetState.Spawnset;

		_buttonV0V1.BackgroundColor = GetFormatBackground(8, 4);
		_buttonV2V3.BackgroundColor = GetFormatBackground(9, 4);
		_buttonV3Next.BackgroundColor = GetFormatBackground(9, 6);

		_buttonSurvival.BackgroundColor = spawnset.GameMode == GameMode.Survival ? _selected : Color.Black;
		_buttonTimeAttack.BackgroundColor = spawnset.GameMode == GameMode.TimeAttack ? _selected : Color.Black;
		_buttonRace.BackgroundColor = spawnset.GameMode == GameMode.Race ? _selected : Color.Black;

		bool practice = spawnset.SpawnVersion > 4;
		_buttonLevel1.IsActive = practice;
		_buttonLevel2.IsActive = practice;
		_buttonLevel3.IsActive = practice;
		_buttonLevel4.IsActive = practice;

		_labelAdditionalGems.IsActive = practice;
		_textInputAdditionalGems.IsActive = practice;
		_textInputAdditionalGems.SetTextIfDeselected(spawnset.AdditionalGems.ToString());

		bool timerStart = spawnset.SpawnVersion > 5;
		_labelTimerStart.IsActive = timerStart;
		_textInputTimerStart.IsActive = timerStart;
		_textInputTimerStart.SetTextIfDeselected(spawnset.TimerStart.ToString("0.0000"));

		_textInputShrinkStart.SetTextIfDeselected(spawnset.ShrinkStart.ToString("0.0"));
		_textInputShrinkEnd.SetTextIfDeselected(spawnset.ShrinkEnd.ToString("0.0"));
		_textInputShrinkRate.SetTextIfDeselected(spawnset.ShrinkRate.ToString("0.000"));
		_textInputBrightness.SetTextIfDeselected(spawnset.Brightness.ToString("0.0"));

		_buttonLevel1.BackgroundColor = GetBackground(HandLevel.Level1);
		_buttonLevel2.BackgroundColor = GetBackground(HandLevel.Level2);
		_buttonLevel3.BackgroundColor = GetBackground(HandLevel.Level3);
		_buttonLevel4.BackgroundColor = GetBackground(HandLevel.Level4);

		Color GetBackground(HandLevel handLevel) => handLevel == spawnset.HandLevel ? GetColor(handLevel) : Color.Black;

		Color GetFormatBackground(int worldVersion, int spawnVersion)
		{
			return worldVersion == spawnset.WorldVersion && spawnVersion == spawnset.SpawnVersion ? _selected : Color.Black;
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
