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

	private readonly SpawnsetTextInput _textInputAdditionalGems;
	private readonly SpawnsetTextInput _textInputTimerStart;

	public SettingsWrapper(Rectangle metric)
		: base(metric)
	{
		const int width = 224;
		const int halfWidth = width / 2;
		int thirdWidth = (int)MathF.Ceiling(width / 3f);
		const int quarterWidth = halfWidth / 2;
		const int height = 20;

		_buttonV0V1 = CreateFormatButton(0, 8, 4);
		_buttonV2V3 = CreateFormatButton(1, 9, 4);
		_buttonV3Next = CreateFormatButton(2, 9, 6);

		_buttonSurvival = CreateGameModeButton(GameMode.Survival);
		_buttonTimeAttack = CreateGameModeButton(GameMode.TimeAttack);
		_buttonRace = CreateGameModeButton(GameMode.Race);

		_buttonLevel1 = CreateHandButton(HandLevel.Level1);
		_buttonLevel2 = CreateHandButton(HandLevel.Level2);
		_buttonLevel3 = CreateHandButton(HandLevel.Level3);
		_buttonLevel4 = CreateHandButton(HandLevel.Level4);

		_labelAdditionalGems = CreateLabel("Addit. gems", 0, height * 3);
		_labelTimerStart = CreateLabel("Timer start", 0, height * 4);

		_textInputAdditionalGems = CreateTextInput(0, height * 3, SpawnsetEditType.AdditionalGems, s => SpawnsetSettingEditUtils.ChangeSetting<int>(v => StateManager.SpawnsetState.Spawnset with { AdditionalGems = v }, s));
		_textInputTimerStart = CreateTextInput(0, height * 4, SpawnsetEditType.TimerStart, s => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { TimerStart = v }, s));

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

		NestingContext.Add(_labelAdditionalGems);
		NestingContext.Add(_labelTimerStart);

		NestingContext.Add(_textInputAdditionalGems);
		NestingContext.Add(_textInputTimerStart);

		Button CreateFormatButton(int index, int worldVersion, int spawnVersion)
		{
			string str = SpawnsetBinary.GetGameVersionString(worldVersion, spawnVersion);
			return new(Rectangle.At(index * thirdWidth, 0, thirdWidth, height), UpdateFormat, Color.Black, Color.White, _hover, Color.White, str, TextAlign.Middle, 2, FontSize.F8X8);

			void UpdateFormat()
			{
				StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with { WorldVersion = worldVersion, SpawnVersion = spawnVersion });
				SpawnsetHistoryManager.Save(SpawnsetEditType.Format);
			}
		}

		Button CreateGameModeButton(GameMode gameMode)
		{
			int index = (int)gameMode;
			return new(Rectangle.At(index * thirdWidth, height, thirdWidth, height), UpdateFormat, Color.Black, Color.White, _hover, Color.White, ToShortString(), TextAlign.Middle, 2, FontSize.F8X8);

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

		Button CreateHandButton(HandLevel handLevel)
		{
			int index = (int)handLevel - 1;
			return new(Rectangle.At(index * quarterWidth, height * 2, quarterWidth, height), UpdateHand, Color.Black, GetColor(handLevel), _hover, Color.White, ToShortString(), TextAlign.Middle, 2, FontSize.F8X8);

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

		SpawnsetTextInput CreateTextInput(int x, int y, SpawnsetEditType spawnsetEditType, Action<string> onInput)
		{
			return SpawnsetComponentBuilder.CreateSpawnsetTextInput(Rectangle.At(x + halfWidth, y, halfWidth, height), onInput, spawnsetEditType);
		}

		Label CreateLabel(string labelText, int x, int y) => new(Rectangle.At(x, y, halfWidth, height), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
	}

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
