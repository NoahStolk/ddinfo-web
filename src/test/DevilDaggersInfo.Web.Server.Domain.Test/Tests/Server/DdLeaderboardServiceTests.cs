using DevilDaggersInfo.Web.Server.Clients.Leaderboard;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.Server;

internal sealed class DdLeaderboardServiceTests : IDisposable
{
	// Zeroes are a valid (empty) get_scores response.
	private static readonly byte[] _emptyLeaderboardResponse = new byte[128];

	private readonly ManualTimeProvider _timeProvider = new();
	private readonly StubHandler _handler = new();
	private readonly HttpClient _httpClient;
	private readonly DdLeaderboardService _service;

	public DdLeaderboardServiceTests()
	{
		_httpClient = new HttpClient(_handler);
		_service = new DdLeaderboardService(_httpClient, new DdLeaderboardCircuitBreaker(_timeProvider), NullLogger<DdLeaderboardService>.Instance);
	}

	private Func<Task> FetchLeaderboard => async () => await _service.GetLeaderboard(1, 100);

	private Func<Task> FetchEntry => async () => await _service.GetEntryById(1);

	public void Dispose()
	{
		_httpClient.Dispose();
		_handler.Dispose();
	}

	[Test]
	public async Task ConnectionFailure_KeepsInnerException()
	{
		_handler.Respond = () => throw new HttpRequestException("Connection failed");

		DdLeaderboardException? exception = await Assert.That(FetchLeaderboard)
			.Throws<DdLeaderboardException>()
			.WithMessage("The leaderboard servers could not be reached.");

		await Assert.That(exception?.InnerException).IsTypeOf<HttpRequestException>();
		await Assert.That(exception?.StatusCode).IsEqualTo(HttpStatusCode.ServiceUnavailable);
	}

	[Test]
	public async Task UnsuccessfulResponse_Throws()
	{
		_handler.Respond = () => new HttpResponseMessage(HttpStatusCode.BadGateway);

		await Assert.That(FetchLeaderboard)
			.Throws<DdLeaderboardException>()
			.WithMessageContaining("HTTP 502");
	}

	[Test]
	public async Task InvalidResponse_ThrowsParseError()
	{
		_handler.Respond = () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent([]) };

		DdLeaderboardException? exception = await Assert.That(FetchLeaderboard)
			.Throws<DdLeaderboardException>()
			.WithMessage("The response from the leaderboard servers could not be parsed.");

		await Assert.That(exception?.InnerException).IsTypeOf<EndOfStreamException>();
	}

	[Test]
	public async Task ConsecutiveFailures_FailFastWithoutContactingServers()
	{
		_handler.Respond = () => throw new HttpRequestException("Connection failed");

		for (int i = 0; i < DdLeaderboardCircuitBreaker.FailureThreshold; i++)
			await Assert.That(FetchLeaderboard).Throws<DdLeaderboardException>();

		await Assert.That(_handler.RequestCount).IsEqualTo(DdLeaderboardCircuitBreaker.FailureThreshold);

		await Assert.That(FetchEntry)
			.Throws<DdLeaderboardException>()
			.WithMessage("The leaderboard servers are currently unavailable. Please try again later.");
		await Assert.That(_handler.RequestCount).IsEqualTo(DdLeaderboardCircuitBreaker.FailureThreshold);
	}

	[Test]
	public async Task OpenCircuit_ProbesAfterBreakDurationAndRecovers()
	{
		_handler.Respond = () => throw new HttpRequestException("Connection failed");
		for (int i = 0; i < DdLeaderboardCircuitBreaker.FailureThreshold; i++)
			await Assert.That(FetchLeaderboard).Throws<DdLeaderboardException>();

		// The probe fails, which opens the circuit for another break duration.
		_timeProvider.Advance(DdLeaderboardCircuitBreaker.BreakDuration);
		await Assert.That(FetchLeaderboard).Throws<DdLeaderboardException>();
		await Assert.That(_handler.RequestCount).IsEqualTo(DdLeaderboardCircuitBreaker.FailureThreshold + 1);

		await Assert.That(FetchLeaderboard).Throws<DdLeaderboardException>();
		await Assert.That(_handler.RequestCount).IsEqualTo(DdLeaderboardCircuitBreaker.FailureThreshold + 1);

		// The next probe succeeds, which closes the circuit.
		_handler.Respond = () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(_emptyLeaderboardResponse) };
		_timeProvider.Advance(DdLeaderboardCircuitBreaker.BreakDuration);
		await FetchLeaderboard();
		await FetchLeaderboard();

		await Assert.That(_handler.RequestCount).IsEqualTo(DdLeaderboardCircuitBreaker.FailureThreshold + 3);
	}

	[Test]
	public async Task Success_ResetsFailureCount()
	{
		_handler.Respond = () => throw new HttpRequestException("Connection failed");
		for (int i = 0; i < DdLeaderboardCircuitBreaker.FailureThreshold - 1; i++)
			await Assert.That(FetchLeaderboard).Throws<DdLeaderboardException>();

		_handler.Respond = () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(_emptyLeaderboardResponse) };
		await FetchLeaderboard();

		_handler.Respond = () => throw new HttpRequestException("Connection failed");
		for (int i = 0; i < DdLeaderboardCircuitBreaker.FailureThreshold - 1; i++)
			await Assert.That(FetchLeaderboard).Throws<DdLeaderboardException>();

		// Still closed, so this request reaches the servers.
		await Assert.That(FetchLeaderboard).Throws<DdLeaderboardException>();
		await Assert.That(_handler.RequestCount).IsEqualTo(DdLeaderboardCircuitBreaker.FailureThreshold * 2);
	}

	[Test]
	public async Task OpenCircuit_LetsThroughOneProbeAtATime()
	{
		DdLeaderboardCircuitBreaker circuitBreaker = new(_timeProvider);
		for (int i = 0; i < DdLeaderboardCircuitBreaker.FailureThreshold; i++)
		{
			await Assert.That(circuitBreaker.TryAcquire()).IsTrue();
			circuitBreaker.RecordFailure();
		}

		await Assert.That(circuitBreaker.TryAcquire()).IsFalse();

		_timeProvider.Advance(DdLeaderboardCircuitBreaker.BreakDuration);
		await Assert.That(circuitBreaker.TryAcquire()).IsTrue();
		await Assert.That(circuitBreaker.TryAcquire()).IsFalse();

		await Assert.That(circuitBreaker.RecordSuccess()).IsTrue();
		await Assert.That(circuitBreaker.TryAcquire()).IsTrue();
		await Assert.That(circuitBreaker.TryAcquire()).IsTrue();
	}

	[Test]
	public async Task RecordFailure_ReportsTransitions()
	{
		DdLeaderboardCircuitBreaker circuitBreaker = new(_timeProvider);
		for (int i = 0; i < DdLeaderboardCircuitBreaker.FailureThreshold - 1; i++)
			await Assert.That(circuitBreaker.RecordFailure()).IsEqualTo(DdLeaderboardCircuitBreaker.FailureOutcome.Closed);

		await Assert.That(circuitBreaker.RecordFailure()).IsEqualTo(DdLeaderboardCircuitBreaker.FailureOutcome.Opened);
		await Assert.That(circuitBreaker.RecordFailure()).IsEqualTo(DdLeaderboardCircuitBreaker.FailureOutcome.Reopened);
	}

	private sealed class StubHandler : HttpMessageHandler
	{
		public Func<HttpResponseMessage> Respond { get; set; } = () => new HttpResponseMessage(HttpStatusCode.OK);

		public int RequestCount { get; private set; }

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			RequestCount++;
			return Task.FromResult(Respond());
		}
	}

	private sealed class ManualTimeProvider : TimeProvider
	{
		private DateTimeOffset _utcNow = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

		public override DateTimeOffset GetUtcNow() => _utcNow;

		public void Advance(TimeSpan timeSpan) => _utcNow += timeSpan;
	}
}
