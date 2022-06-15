using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.Events;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.ReplayEditor;

public partial class ReplayParser
{
	private List<List<IEvent>> _events = null!;
	private int _currentSecond;

	[Parameter]
	public ReplayBinary ReplayBinary { get; set; } = null!;

	protected override void OnInitialized()
	{
		_events = ReplayEventsParser.ParseCompressedEvents(ReplayBinary.CompressedEvents!);
	}
}
