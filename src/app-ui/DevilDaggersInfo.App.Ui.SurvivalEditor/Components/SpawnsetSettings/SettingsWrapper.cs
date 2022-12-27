using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;
using StateManager = DevilDaggersInfo.App.Ui.Base.StateManagement.StateManager;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSettings;

public class SettingsWrapper : AbstractComponent
{
	private const int _offset = 16;

	private readonly int _width;
	private readonly int _halfWidth;
	private readonly int _thirdWidth;
	private readonly int _quarterWidth;

	private readonly TextButton _buttonV0V1;
	private readonly TextButton _buttonV2V3;
	private readonly TextButton _buttonV3Next;

	private readonly TextButton _buttonSurvival;
	private readonly TextButton _buttonTimeAttack;
	private readonly TextButton _buttonRace;

	private readonly TextButton _buttonLevel1;
	private readonly TextButton _buttonLevel2;
	private readonly TextButton _buttonLevel3;
	private readonly TextButton _buttonLevel4;

	private readonly Label _labelAdditionalGems;
	private readonly Label _labelTimerStart;
	private readonly SpawnsetTextInput _textInputShrinkStart;
	private readonly SpawnsetTextInput _textInputShrinkEnd;
	private readonly SpawnsetTextInput _textInputShrinkRate;
	private readonly SpawnsetTextInput _textInputBrightness;
	private readonly SpawnsetTextInput _textInputAdditionalGems;
	private readonly SpawnsetTextInput _textInputTimerStart;

	public SettingsWrapper(IBounds bounds)
		: base(bounds)
	{
		_width = bounds.Size.X;
		_halfWidth = _width / 2;
		_thirdWidth = (int)MathF.Ceiling(_width / 3f);
		_quarterWidth = _halfWidth / 2;

		Label title = new(bounds.CreateNested(0, 0, _width, 48), "Settings", GlobalStyles.LabelTitle);
		NestingContext.Add(title);

		int y = title.Bounds.Size.Y;
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
		(_textInputShrinkStart, _) = AddSetting("Shrink start", ref y, ChangeShrinkStart);
		(_textInputShrinkEnd, _) = AddSetting("Shrink end", ref y, ChangeShrinkEnd);
		(_textInputShrinkRate, _) = AddSetting("Shrink rate", ref y, ChangeShrinkRate);
		(_textInputBrightness, _) = AddSetting("Brightness", ref y, ChangeBrightness);
		(_textInputAdditionalGems, _labelAdditionalGems) = AddSetting("Addit. gems", ref y, ChangeAdditionalGems);
		(_textInputTimerStart, _labelTimerStart) = AddSetting("Timer start", ref y, ChangeTimerStart);

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

		StateManager.Subscribe<LoadSpawnset>(SetSpawnset);
		StateManager.Subscribe<UpdateSpawnsetSetting>(SetSpawnset);
	}

	private TextButton CreateFormatButton(int y, int index, int worldVersion, int spawnVersion)
	{
		string str = SpawnsetBinary.GetGameVersionString(worldVersion, spawnVersion);
		return new(Bounds.CreateNested(index * _thirdWidth, y, _thirdWidth, _offset), UpdateFormat, GlobalStyles.SpawnsetSetting, GlobalStyles.DefaultMiddle, str);

		void UpdateFormat()
		{
			StateManager.Dispatch(new UpdateSpawnsetSetting(StateManager.SpawnsetState.Spawnset with { WorldVersion = worldVersion, SpawnVersion = spawnVersion }, SpawnsetEditType.Format));
		}
	}

	private TextButton CreateGameModeButton(int y, GameMode gameMode)
	{
		int index = (int)gameMode;
		return new(Bounds.CreateNested(index * _thirdWidth, y, _thirdWidth, _offset), UpdateFormat, GlobalStyles.SpawnsetSetting, GlobalStyles.DefaultMiddle, ToShortString());

		void UpdateFormat()
		{
			StateManager.Dispatch(new UpdateSpawnsetSetting(StateManager.SpawnsetState.Spawnset with { GameMode = gameMode }, SpawnsetEditType.GameMode));
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
		return new(Bounds.CreateNested(index * _quarterWidth, y, _quarterWidth, _offset), UpdateHand, GlobalStyles.HandLevelButtonStyles[handLevel], GlobalStyles.HandLevelText, ToShortString());

		void UpdateHand()
		{
			StateManager.Dispatch(new UpdateSpawnsetSetting(StateManager.SpawnsetState.Spawnset with { HandLevel = handLevel }, SpawnsetEditType.HandLevel));
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

	private (SpawnsetTextInput TextInput, Label Label) AddSetting(string labelText, ref int y, Action<string> onInput)
	{
		Label label = new(Bounds.CreateNested(0, y, _halfWidth, _offset), labelText, GlobalStyles.LabelDefaultLeft);
		SpawnsetTextInput textInput = SpawnsetComponentBuilder.CreateSpawnsetTextInput(Bounds.CreateNested(_halfWidth, y, _halfWidth, _offset), onInput);
		NestingContext.Add(label);
		NestingContext.Add(textInput);
		y += _offset;

		return (textInput, label);
	}

	private static void ChangeAdditionalGems(string input) => ChangeSetting<int>(v => StateManager.SpawnsetState.Spawnset with { AdditionalGems = v }, input, SpawnsetEditType.AdditionalGems);

	private static void ChangeTimerStart(string input) => ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { TimerStart = v }, input, SpawnsetEditType.TimerStart);

	private static void ChangeShrinkStart(string input) => ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkStart = v }, input, SpawnsetEditType.ShrinkStart);

	private static void ChangeShrinkEnd(string input) => ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkEnd = v }, input, SpawnsetEditType.ShrinkEnd);

	private static void ChangeShrinkRate(string input) => ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkRate = v }, input, SpawnsetEditType.ShrinkRate);

	private static void ChangeBrightness(string input) => ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { Brightness = v }, input, SpawnsetEditType.Brightness);

	private static void ChangeSetting<T>(Func<T, SpawnsetBinary> spawnsetBuilder, string input, SpawnsetEditType spawnsetEditType)
		where T : IParsable<T>
	{
		ParseUtils.TryParseAndExecute<T>(input, f => StateManager.Dispatch(new UpdateSpawnsetSetting(spawnsetBuilder(f), spawnsetEditType)));
	}

	private void SetSpawnset()
	{
		SpawnsetBinary spawnset = StateManager.SpawnsetState.Spawnset;

		_buttonV0V1.ButtonStyle = GetFormatBackground(8, 4);
		_buttonV2V3.ButtonStyle = GetFormatBackground(9, 4);
		_buttonV3Next.ButtonStyle = GetFormatBackground(9, 6);

		_buttonSurvival.ButtonStyle = spawnset.GameMode == GameMode.Survival ? GlobalStyles.SelectedSpawnsetSetting : GlobalStyles.SpawnsetSetting;
		_buttonTimeAttack.ButtonStyle = spawnset.GameMode == GameMode.TimeAttack ? GlobalStyles.SelectedSpawnsetSetting : GlobalStyles.SpawnsetSetting;
		_buttonRace.ButtonStyle = spawnset.GameMode == GameMode.Race ? GlobalStyles.SelectedSpawnsetSetting : GlobalStyles.SpawnsetSetting;

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

		_buttonLevel1.ButtonStyle = GetStyle(HandLevel.Level1);
		_buttonLevel2.ButtonStyle = GetStyle(HandLevel.Level2);
		_buttonLevel3.ButtonStyle = GetStyle(HandLevel.Level3);
		_buttonLevel4.ButtonStyle = GetStyle(HandLevel.Level4);

		ButtonStyle GetStyle(HandLevel handLevel) => handLevel == spawnset.HandLevel ? GlobalStyles.SelectedHandLevelButtonStyles[handLevel] : GlobalStyles.HandLevelButtonStyles[handLevel];

		ButtonStyle GetFormatBackground(int worldVersion, int spawnVersion) => worldVersion == spawnset.WorldVersion && spawnVersion == spawnset.SpawnVersion ? GlobalStyles.SelectedSpawnsetSetting : GlobalStyles.SpawnsetSetting;
	}
}
