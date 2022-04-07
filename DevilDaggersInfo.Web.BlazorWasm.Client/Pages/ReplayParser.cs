using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.Enums;
using DevilDaggersInfo.Core.Replay.Events;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages;

public partial class ReplayParser
{
	private List<List<IEvent>>? _events;
	private long _processingTimeInMs;
	private bool _notFound;
	private int _currentSecond;

	[Parameter]
	[EditorRequired]
	public int Id { get; set; }

	[Inject]
	public PublicApiHttpClient Http { get; set; } = null!;

	protected override async Task OnParametersSetAsync()
	{
		byte[] replayBuffer;
		try
		{
			replayBuffer = await Http.GetCustomEntryReplayBufferById(Id);
			_notFound = false;
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			_notFound = true;
			return;
		}

		ReplayBinary replayBinary = new(replayBuffer, ReplayBinaryReadComprehensiveness.All);

		Stopwatch sw = Stopwatch.StartNew();
		_events = ReplayEventsParser.ParseEvents(replayBinary.CompressedEvents!);
		_processingTimeInMs = sw.ElapsedMilliseconds;
		sw.Stop();
	}
}
