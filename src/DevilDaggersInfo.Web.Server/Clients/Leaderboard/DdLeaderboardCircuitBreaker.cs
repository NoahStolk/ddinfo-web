namespace DevilDaggersInfo.Web.Server.Clients.Leaderboard;

/// <summary>
/// Stops requests to the leaderboard servers after several consecutive failures, so an outage doesn't make every request wait for a timeout (and log an error).
/// Once the break duration has passed, a single request is let through to probe whether the servers are back.
/// </summary>
internal sealed class DdLeaderboardCircuitBreaker(TimeProvider timeProvider)
{
	public const int FailureThreshold = 3;

	public static readonly TimeSpan BreakDuration = TimeSpan.FromMinutes(1);

	private readonly Lock _lock = new();

	private int _consecutiveFailures;
	private DateTimeOffset _retryAt;
	private bool _probeInFlight;

	internal enum FailureOutcome
	{
		/// <summary>The circuit is still closed; the failure threshold has not been reached yet.</summary>
		Closed,

		/// <summary>This failure reached the threshold and opened the circuit.</summary>
		Opened,

		/// <summary>The circuit was already open and the probe request failed.</summary>
		Reopened,
	}

	/// <summary>
	/// Returns <see langword="false"/> when the request should fail immediately without contacting the leaderboard servers.
	/// Every acquired request must be followed by a call to <see cref="RecordSuccess"/> or <see cref="RecordFailure"/>.
	/// </summary>
	public bool TryAcquire()
	{
		lock (_lock)
		{
			if (_consecutiveFailures < FailureThreshold)
				return true;

			if (_probeInFlight || timeProvider.GetUtcNow() < _retryAt)
				return false;

			_probeInFlight = true;
			return true;
		}
	}

	/// <summary>
	/// Returns <see langword="true"/> when the circuit was open, meaning the leaderboard servers have recovered.
	/// </summary>
	public bool RecordSuccess()
	{
		lock (_lock)
		{
			bool wasOpen = _consecutiveFailures >= FailureThreshold;
			_consecutiveFailures = 0;
			_probeInFlight = false;
			return wasOpen;
		}
	}

	public FailureOutcome RecordFailure()
	{
		lock (_lock)
		{
			_consecutiveFailures++;
			_probeInFlight = false;

			if (_consecutiveFailures < FailureThreshold)
				return FailureOutcome.Closed;

			_retryAt = timeProvider.GetUtcNow() + BreakDuration;
			return _consecutiveFailures == FailureThreshold ? FailureOutcome.Opened : FailureOutcome.Reopened;
		}
	}
}
