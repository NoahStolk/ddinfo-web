using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class BucketToolSettingsWrapper : AbstractComponent
{
	public BucketToolSettingsWrapper(IBounds bounds)
		: base(bounds)
	{
		int y = 0;
		AddSetting("Tolerance", 0, ref y, ChangeBucketTolerance, StateManager.ArenaEditorState.BucketTolerance.ToString("0.0"));
		AddSetting("Void height", 0, ref y, ChangeBucketVoidHeight, StateManager.ArenaEditorState.BucketVoidHeight.ToString("0.0"));

		void AddSetting(string labelText, int x, ref int y, Action<string> onInput, string initialInput)
		{
			int halfWidth = Bounds.Size.X / 2;
			Label label = new(Rectangle.At(x, y, halfWidth, 16), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
			TextInput textInput = new(Rectangle.At(x + halfWidth, y, halfWidth, 16), true, onInput, onInput, onInput, GlobalStyles.TextInput);
			textInput.KeyboardInput.SetText(initialInput);
			NestingContext.Add(label);
			NestingContext.Add(textInput);
			y += 16;
		}

		static void ChangeBucketTolerance(string s) => ParseUtils.TryParseAndExecute<float>(s, 0, StateManager.SetArenaBucketTolerance);

		static void ChangeBucketVoidHeight(string s) => ParseUtils.TryParseAndExecute<float>(s, StateManager.SetArenaBucketVoidHeight);
	}
}
