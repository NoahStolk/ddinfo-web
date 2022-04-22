using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.Enums;
using DevilDaggersInfo.Core.Replay.Events;
using DevilDaggersInfo.Web.Client.HttpClients;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Client.Pages;

public partial class ReplayParser
{
	private ReplayBinary? _replayBinary;
	private List<List<IEvent>>? _events;
	private long _processingTimeInMs;
	private int _fileSize;
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
			_fileSize = replayBuffer.Length;
			_notFound = false;
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			_fileSize = 0;
			_notFound = true;
			return;
		}

		Stopwatch sw = Stopwatch.StartNew();
		_replayBinary = new(replayBuffer, ReplayBinaryReadComprehensiveness.All);
		_events = ReplayEventsParser.ParseCompressedEvents(_replayBinary.CompressedEvents!);
		_processingTimeInMs = sw.ElapsedMilliseconds;
		sw.Stop();
	}
}
