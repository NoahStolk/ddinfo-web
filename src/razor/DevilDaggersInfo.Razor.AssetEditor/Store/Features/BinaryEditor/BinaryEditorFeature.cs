using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Razor.AssetEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.AssetEditor.Store.Features.BinaryEditor;

public class BinaryEditorFeature : Feature<BinaryEditorState>
{
	public override string GetName() => "Binary editor";

	protected override BinaryEditorState GetInitialState() => new(new(ModBinaryType.Dd), "(Untitled)");
}
