using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
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
			Label label = new(bounds.CreateNested(x, y, halfWidth, 16), labelText, LabelStyles.DefaultLeft);
			TextInput textInput = new(bounds.CreateNested(x + halfWidth, y, halfWidth, 16), true, onInput, onInput, onInput, TextInputStyles.Default);
			textInput.KeyboardInput.SetText(initialInput);
			NestingContext.Add(label);
			NestingContext.Add(textInput);
			y += 16;
		}

		static void ChangeBucketTolerance(string s) => ParseUtils.TryParseAndExecute<float>(s, 0, f => StateManager.Dispatch(new SetArenaBucketTolerance(f)));

		static void ChangeBucketVoidHeight(string s) => ParseUtils.TryParseAndExecute<float>(s, f => StateManager.Dispatch(new SetArenaBucketVoidHeight(f)));
	}
}
