using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.Enums;

namespace DevilDaggersInfo.Razor.Core.ReplayEditor.Pages;

public partial class ReplayParserPage
{
	private ReplayBinary? _replayBinary;

	public void Load(byte[] file)
	{
		_replayBinary = new(file, ReplayBinaryReadComprehensiveness.All);
	}
}
