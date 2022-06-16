using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.Enums;

namespace DevilDaggersInfo.Razor.Core.ReplayEditor.Pages;

public partial class ReplayParserPage
{
	private ReplayBinary? _replayBinary;

	public void OpenReplay()
	{
		byte[] bytes = File.ReadAllBytes(@"C:\Users\NOAH\AppData\Roaming\DevilDaggers\replays\dagger-down-test_5.56-xvlv-0be523f3[22a5f347].ddreplay");
		_replayBinary = new(bytes, ReplayBinaryReadComprehensiveness.All);
	}
}
